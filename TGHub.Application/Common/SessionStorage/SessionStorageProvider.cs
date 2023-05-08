using AutoMapper;
using Blazored.SessionStorage;
using TGHub.Application.Services.Base;
using TGHub.Domain.Entities;

namespace TGHub.Application.Common.SessionStorage;

public class SessionStorageProvider
{
    private readonly IService<ChannelAdministrator> _channelAdministratorService;
    private readonly LocalStorageProvider _localStorageProvider;
    private readonly IMapper _mapper;
    private readonly ISessionStorageService _sessionStorageService;

    public SessionStorageProvider(ISessionStorageService sessionStorageService,
        LocalStorageProvider localStorageProvider,
        IService<ChannelAdministrator> channelAdministratorService, IMapper mapper)
    {
        _sessionStorageService = sessionStorageService;
        _localStorageProvider = localStorageProvider;
        _channelAdministratorService = channelAdministratorService;
        _mapper = mapper;
    }

    public ChannelAdministratorSessionData? SelectedChannelAdministratorData { get; set; }

    public async Task FetchAsync()
    {
        var selectedChannelId = await _sessionStorageService
            .GetItemAsync<int?>(nameof(ChannelAdministratorSessionData.ChannelId));
        if (selectedChannelId == null)
        {
            SelectedChannelAdministratorData = null;
            return;
        }

        var channelAdministrator = await _channelAdministratorService
            .FirstOrDefaultAsync(ch => ch.AdministratorId == _localStorageProvider.Id &&
                                      ch.ChannelId == selectedChannelId &&
                                      ch.IsActive);
        if (channelAdministrator == null)
        {
            SelectedChannelAdministratorData = null;
            await PushAsync();
        }

        SelectedChannelAdministratorData = _mapper.Map<ChannelAdministratorSessionData>(channelAdministrator);
    }

    public async Task PushAsync()
    {
        await _sessionStorageService.SetItemAsync(nameof(ChannelAdministratorSessionData.ChannelId),
            SelectedChannelAdministratorData?.ChannelId);
    }
}