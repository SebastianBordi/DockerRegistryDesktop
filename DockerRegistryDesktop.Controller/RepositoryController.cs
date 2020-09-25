using DockerRegistryDesktop.Controller.DTOs;
using DockerRegistryDesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DockerRegistryDesktop.Controller
{
    public class RepositoryController
    {
        private HttpClient _client = new HttpClient();
        private static RepositoryController _instance;

        private RepositoryController(string server, string user, string password)
        {
            this._client.BaseAddress = new Uri(server);
            this._client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var byteArray = Encoding.ASCII.GetBytes($"{user}:{password}");
            this._client.DefaultRequestHeaders.Authorization 
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        public static RepositoryController GetInstance(string server, string user, string password)
        {
            if (_instance == null)
            {
                _instance = new RepositoryController(server, user, password);
            }
            return _instance;
        }
        public static async Task<bool> TestServer(string server, string user, string password)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(server);
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            var byteArray = Encoding.ASCII.GetBytes($"{user}:{password}");
            httpClient.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                var response = await httpClient.GetAsync("/v2/");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    throw new Exception($"Could not connect to the server - {response.StatusCode}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Repository>> GetRepositoriesAsync()
        {
            var repositories = new List<Repository>();
            try
            {
                var response = await _client.GetAsync("/v2/_catalog");
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var repos = JsonConvert.DeserializeObject<RepositoryResponse>(await response.Content.ReadAsStringAsync());
                    foreach (var repo in repos.RepositoryNames)
                    {
                        Repository repository = new Repository();
                        repository.Name = repo;
                        repository.Tags = await GetTagsAsync(repo);
                        if(repository.Tags != null)
                            repositories.Add(repository);
                    }
                    return repositories;
                }
                else 
                    throw new Exception($"Error Getting Repositories {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<Tag[]> GetTagsAsync(string repo)
        {
            Tag[] tags;
            try
            {
                if (_client.DefaultRequestHeaders.Contains("Accept")) _client.DefaultRequestHeaders.Remove("Accept");
                _client.DefaultRequestHeaders.Add("Accept", "application/vnd.docker.distribution.manifest.v2+json");
                var response = await _client.GetAsync($"/v2/{repo}/tags/list");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var repositoryTags = JsonConvert.DeserializeObject<TagResponse>(await response.Content.ReadAsStringAsync());
                    if (repositoryTags.Tags == null)
                        return null;
                    tags = new Tag[repositoryTags.Tags.Count()];
                    for (int i = 0; i < repositoryTags.Tags.Count(); i++)
                    {
                        Tag tag = new Tag();
                        tag.Name = repositoryTags.Tags[i];
                        //tag.Digest = await GetDigestAsync(tag.Name, repo);
                        tags[i] = tag;
                    }

                    if (_client.DefaultRequestHeaders.Contains("Accept")) _client.DefaultRequestHeaders.Remove("Accept");
                    return tags;
                }
                else
                {
                    if (_client.DefaultRequestHeaders.Contains("Accept")) _client.DefaultRequestHeaders.Remove("Accept");
                    throw new Exception($"Error Getting Tags repo :  {repo} {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<string> GetDigestAsync(string tag, string repo)
        {
            try
            {
                var response = await _client.GetAsync($"/v2/{repo}/manifests/{tag}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var digest = response.Headers.GetValues("Docker-Content-Digest").FirstOrDefault() ;
                    if(digest == null)
                        throw new Exception($"Error Getting Diget {repo}:{tag}");
                    return digest;
                }
                throw new Exception($"Error Getting Diget {repo}:{tag} {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
