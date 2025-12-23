using PluginManager.Api;
using PluginManager.Api.Capabilities.Implementations.Events.GameEvents;
using PluginManager.Api.Capabilities.Implementations.Utils;
using PluginManager.Api.Contracts;
using PluginManager.Api.Hooks;

namespace TileClaimProtector;

public class TileClaimProtector : BasePlugin
{
    public override string ModuleName => "TileClaimProtector";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "TouchMe-Inc";
    public override string ModuleDescription => "Blocks unauthorized access to objects inside other players land claims";

    protected override void OnLoad()
    {
        RegisterEventHandler<TileEntityAccessAttemptEvent>(OnTileEntityAccessAttempt, HookMode.Pre);
    }

    private HookResult OnTileEntityAccessAttempt(TileEntityAccessAttemptEvent evt)
    {
        var claimStatus = Capabilities.Get<IPlayerUtil>().GetClaimOwner(evt.EntityId, evt.TileEntityPosition);

        return claimStatus == LandClaimOwner.Other ? HookResult.Handled : HookResult.Continue;
    }
}