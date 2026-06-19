using UnityEngine;

public sealed class PrototypeHud : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private DefenseObjective objective;
    private CastleGateObjective castleGate;
    private PlayerHitscanWeapon weapon;
    private WaveDirector waveDirector;
    private GrapplingHookPrototype grapplingHook;
    private GUIStyle labelStyle;
    private GUIStyle centerStyle;
    private GUIStyle panelStyle;
    private GUIStyle smallStyle;

    public void Initialize(
        PlayerHealth player,
        DefenseObjective defenseObjective,
        PlayerHitscanWeapon playerWeapon,
        WaveDirector director,
        CastleGateObjective gateObjective = null)
    {
        playerHealth = player;
        objective = defenseObjective;
        weapon = playerWeapon;
        waveDirector = director;
        castleGate = gateObjective;
        grapplingHook = player != null ? player.GetComponent<GrapplingHookPrototype>() : null;
    }

    private void OnGUI()
    {
        EnsureStyles();

        DrawCrosshair();
        DrawStatus();
        DrawWeaponFeedback();
        DrawWaveMessage();

        if (waveDirector != null && waveDirector.GameOver)
        {
            GUI.Label(new Rect(0f, Screen.height * 0.42f, Screen.width, 120f), "방어 실패", centerStyle);
        }
    }

    private void DrawCrosshair()
    {
        var size = 18f + ((weapon != null ? weapon.Feedback01 : 0f) * 14f);
        var centerX = Screen.width * 0.5f;
        var centerY = Screen.height * 0.5f;
        var crosshairColor = Color.white;
        if (weapon != null && weapon.KillFeedback01 > 0f)
        {
            crosshairColor = new Color(1f, 0.18f, 0.08f);
        }
        else if (weapon != null && weapon.HitFeedback01 > 0f)
        {
            crosshairColor = new Color(1f, 0.86f, 0.1f);
        }

        centerStyle.normal.textColor = crosshairColor;
        GUI.Label(new Rect(centerX - size, centerY - size, size * 2f, size * 2f), "+", centerStyle);
        centerStyle.normal.textColor = new Color(0.95f, 0.12f, 0.08f);
    }

    private void DrawStatus()
    {
        var playerHp = playerHealth != null ? $"{playerHealth.CurrentHealth:0}/{playerHealth.MaxHealth:0}" : "-";
        var objectiveName = objective != null ? objective.DisplayName : "왕";
        var objectiveHp = objective != null ? $"{objective.CurrentHealth:0}/{objective.MaxHealth:0}" : "-";
        var gateText = castleGate != null
            ? $"{castleGate.DisplayName} {(castleGate.IsAlive ? $"{castleGate.CurrentHealth:0}/{castleGate.MaxHealth:0}" : "돌파됨")}\n"
            : string.Empty;
        var ammo = weapon != null ? $"{weapon.AmmoInMagazine}/{weapon.MagazineSize}" : "-";
        var reload = weapon != null && weapon.IsReloading ? " 재장전" : string.Empty;
        var weaponName = weapon != null ? weapon.WeaponName : "증기 리볼버";
        var armSkillName = weapon != null ? weapon.ArmSkillName : "기계 왼팔 전격";
        var wave = waveDirector != null ? waveDirector.CurrentWave : 0;
        var enemies = waveDirector != null ? waveDirector.AliveEnemyCount + waveDirector.EnemiesRemainingToSpawn : 0;
        var nextWave = waveDirector != null && enemies == 0 ? $" / 다음 {waveDirector.NextWaveTimer:0.0}s" : string.Empty;

        GUI.Box(new Rect(18f, 18f, 470f, 178f), GUIContent.none, panelStyle);
        GUI.Label(new Rect(34f, 28f, 440f, 160f), $"플레이어 {playerHp}\n{objectiveName} 체력 {objectiveHp}\n{gateText}무기 {weaponName}\n왼팔 {armSkillName}\n탄약 {ammo}{reload}\n웨이브 {wave} / 남은 적 {enemies}{nextWave}", labelStyle);

        GUI.Box(new Rect(Screen.width - 330f, Screen.height - 118f, 306f, 92f), GUIContent.none, panelStyle);
        var grapple = grapplingHook != null
            ? (grapplingHook.IsGrappling ? "그래플 이동 중" : $"그래플 쿨타임 {grapplingHook.Cooldown01 * 100f:0}%")
            : "그래플 없음";
        GUI.Label(new Rect(Screen.width - 312f, Screen.height - 102f, 285f, 76f), $"좌클릭 사격  우클릭 그래플\nR 재장전  L 커서\n{grapple}", smallStyle);
    }

    private void DrawWeaponFeedback()
    {
        if (weapon == null)
        {
            return;
        }

        if (weapon.EmptyFeedback01 > 0f)
        {
            GUI.Label(new Rect(0f, Screen.height * 0.62f, Screen.width, 48f), "탄약 없음 - 재장전", centerStyle);
            return;
        }

        if (weapon.IsReloading)
        {
            GUI.Label(new Rect(0f, Screen.height * 0.62f, Screen.width, 48f), $"재장전 {weapon.ReloadProgress01 * 100f:0}%", centerStyle);
            return;
        }

        if (weapon.KillFeedback01 > 0f)
        {
            GUI.Label(new Rect(0f, Screen.height * 0.58f, Screen.width, 48f), "처치", centerStyle);
        }
        else if (weapon.HitFeedback01 > 0f)
        {
            GUI.Label(new Rect(0f, Screen.height * 0.58f, Screen.width, 48f), "명중", centerStyle);
        }
    }

    private void DrawWaveMessage()
    {
        if (waveDirector == null || string.IsNullOrEmpty(waveDirector.WaveMessage))
        {
            return;
        }

        GUI.Label(new Rect(0f, Screen.height * 0.18f, Screen.width, 70f), waveDirector.WaveMessage, centerStyle);
    }

    private void EnsureStyles()
    {
        if (labelStyle != null)
        {
            return;
        }

        labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            normal = { textColor = new Color(0.9f, 0.86f, 0.72f) }
        };

        centerStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 34,
            normal = { textColor = new Color(0.95f, 0.12f, 0.08f) }
        };

        smallStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            normal = { textColor = new Color(0.9f, 0.86f, 0.72f) }
        };

        panelStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = Texture2D.grayTexture }
        };
    }
}
