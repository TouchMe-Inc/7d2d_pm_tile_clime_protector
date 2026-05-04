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

    public override string ModuleDescription =>
        "Blocks unauthorized access to objects inside other players land claims";

    private IGameUtil _gameUtil;
    private IPlayerUtil _playerUtil;

    protected override void OnLoad()
    {
        _gameUtil = Capabilities.Get<IGameUtil>();
        _playerUtil = Capabilities.Get<IPlayerUtil>();
        RegisterEventHandler<TileEntityAccessAttemptEvent>(OnTileEntityAccessAttempt, HookMode.Pre);
    }

    private HookResult OnTileEntityAccessAttempt(TileEntityAccessAttemptEvent evt)
    {
        if (evt.TileEntity.Type == TileEntityType.Loot
            && evt.TileEntity.Id != -1
            && _gameUtil.GetEntityType(evt.TileEntity.Id).Equals("EntityBackpack"))
        {
            return HookResult.Continue;
        }

        var claimStatus = _playerUtil.GetClaimOwner(evt.EntityId, evt.TileEntity.Position);
        if (claimStatus != LandClaimOwner.Other) return HookResult.Continue;

        _playerUtil.PlaySound(evt.EntityId, "ui_denied", 20);
        return HookResult.Handled;
    }
}