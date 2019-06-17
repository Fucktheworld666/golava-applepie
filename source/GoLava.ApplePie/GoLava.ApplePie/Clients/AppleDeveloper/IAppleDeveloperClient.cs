using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts.AppleDeveloper;

namespace GoLava.ApplePie.Clients.AppleDeveloper
{
    public interface IAppleDeveloperClient : IClientBase
    {
        Task<Application> AddApplicationAsync(ClientContext context, NewApplication addApplication, Platform platform = Platform.Ios);
        Task<Application> AddApplicationAsync(ClientContext context, string teamId, NewApplication newApplication, Platform platform = Platform.Ios);
        Task<List<Device>> AddDeviceAsync(ClientContext context, string teamId, string uiid, string name, DeviceClass deviceClass, Platform platform = Platform.Ios);
        Task<Device> ChangeDeviceNameAsync(ClientContext context, Device device, string newName);
        Task<bool> DeleteApplicationAsync(ClientContext context, Application application);
        Task<bool> DisableDeviceAsync(ClientContext context, Device device);
        Task<X509Certificate2> DownloadCertificateAsync(ClientContext context, CertificateRequest certificateRequest);
        Task<byte[]> DownloadRawCertificateAsync(ClientContext context, CertificateRequest certificateRequest);
        Task<Device> EnableDeviceAsync(ClientContext context, Device device);
        Task<ApplicationDetails> GetApplicationDetails(ClientContext context, Application application);
        Task<List<Application>> GetApplicationsAsync(ClientContext context, Platform platform = Platform.Ios);
        Task<List<Application>> GetApplicationsAsync(ClientContext context, string teamId, Platform platform = Platform.Ios);
        Task<List<CertificateRequest>> GetCertificateRequestsAsync(ClientContext context, Platform platform = Platform.Ios);
        Task<List<CertificateRequest>> GetCertificateRequestsAsync(ClientContext context, string teamId, Platform platform = Platform.Ios);
        Task<List<Device>> GetDevicesAsync(ClientContext context, Platform platform = Platform.Ios);
        Task<List<Device>> GetDevicesAsync(ClientContext context, string teamId, Platform platform = Platform.Ios);
        Task<List<Team>> GetTeamsAsync(ClientContext context);
        Task<CertificateRequest> RevokeCertificateRequestAsync(ClientContext context, CertificateRequest certificateRequest);
        Task<CertificateRequest> SubmitCertificateSigningRequestAsync(ClientContext context, CertificateSigningRequest certSigningRequest, Platform platform = Platform.Ios);
        Task<CertificateRequest> SubmitCertificateSigningRequestAsync(ClientContext context, string teamId, CertificateSigningRequest certSigningRequest, Platform platform = Platform.Ios);
        Task<ApplicationDetails> UpdateApplicationFeatureAsync<TFeatureValue>(ClientContext context, ApplicationDetails applicationDetails, Expression<Func<ApplicationFeatures, TFeatureValue>> feature, TFeatureValue value);
    }
}