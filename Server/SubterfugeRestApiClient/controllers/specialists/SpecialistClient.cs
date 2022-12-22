﻿using SubterfugeCore.Models.GameEvents;
using SubterfugeCore.Models.GameEvents.Api;
using SubterfugeRestApiClient.controllers.exception;

namespace SubterfugeRestApiClient.controllers.specialists;

public class SpecialistClient : ISubterfugeCustomSpecialistApi, ISubterfugeSpecialistPackageApi
{
    private HttpClient client;

    public SpecialistClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CreateSpecialistPackageResponse> CreateSpecialistPackage(CreateSpecialistPackageRequest request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"api/specialist/package/create", request);
        if (!response.IsSuccessStatusCode)
        {
            throw await SubterfugeClientException.CreateFromResponseMessage(response);
        }
        return await response.Content.ReadAsAsync<CreateSpecialistPackageResponse>();
    }
    
    public async Task<GetSpecialistPackagesResponse> GetSpecialistPackages(GetSpecialistPackagesRequest request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"api/specialist/packages", request);
        if (!response.IsSuccessStatusCode)
        {
            throw await SubterfugeClientException.CreateFromResponseMessage(response);
        }
        return await response.Content.ReadAsAsync<GetSpecialistPackagesResponse>();
    }
    
    public async Task<GetSpecialistPackagesResponse> GetSpecialistPackages(string packageId)
    {
        HttpResponseMessage response = await client.GetAsync($"api/specialist/package/{packageId}");
        if (!response.IsSuccessStatusCode)
        {
            throw await SubterfugeClientException.CreateFromResponseMessage(response);
        }
        return await response.Content.ReadAsAsync<GetSpecialistPackagesResponse>();
    }

    public async Task<SubmitCustomSpecialistResponse> SubmitCustomSpecialist(SubmitCustomSpecialistRequest request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"api/specialist/create", request);
        if (!response.IsSuccessStatusCode)
        {
            throw await SubterfugeClientException.CreateFromResponseMessage(response);
        }
        return await response.Content.ReadAsAsync<SubmitCustomSpecialistResponse>();
    }

    public async Task<GetCustomSpecialistsResponse> GetCustomSpecialists(GetCustomSpecialistsRequest request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"api/specialists", request);
        if (!response.IsSuccessStatusCode)
        {
            throw await SubterfugeClientException.CreateFromResponseMessage(response);
        }
        return await response.Content.ReadAsAsync<GetCustomSpecialistsResponse>();
    }

    public async Task<GetCustomSpecialistsResponse> GetCustomSpecialist(string specialistId)
    {
        HttpResponseMessage response = await client.GetAsync($"api/specialist/{specialistId}");
        if (!response.IsSuccessStatusCode)
        {
            throw await SubterfugeClientException.CreateFromResponseMessage(response);
        }
        return await response.Content.ReadAsAsync<GetCustomSpecialistsResponse>();
    }
}