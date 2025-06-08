using System;
using System.Collections.Generic;
using Gameplay.MVVMGroups.Test;
using MVVMCameraMovement;
using MVVMZoomCamera;
using PlayerController.MVVM.Player;
using SpawningSpheresGame.Game.Common.DataTypes.Enums;
using UnityEngine;

namespace SpawningSpheresGame.Utils.GameplayUtils
{
    public static class EntityBinderTypeRegistry
    {
        private static readonly Dictionary<GameplayEntitiesId, Type> BinderTypes = new()
    {
        { GameplayEntitiesId.Player, typeof(PlayerBinder) },
        { GameplayEntitiesId.Test, typeof(TestBinder) },
        { GameplayEntitiesId.AdvancedPlayer, typeof(AdvancedPlayerBinder) },
        { GameplayEntitiesId.MoveRtsCamera, typeof(MoveRtsCameraBinder) },
        {GameplayEntitiesId.ZoomRtsCamera,typeof(ZoomRtsCameraBinder)}
    };

        public static Type GetBinderType(GameplayEntitiesId entityId)
        {
            if (!BinderTypes.TryGetValue(entityId, out var binderType))
            {
                Debug.LogWarning($"No binder type registered for entity {entityId}. Using default.");
                return null;
            }
            return binderType;
        }
    }
}