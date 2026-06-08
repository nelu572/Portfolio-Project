using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public sealed class PlayerHitscanWeapon : MonoBehaviour
{
    [Header("무기")]
    [SerializeField] private Camera aimCamera;
    [SerializeField] private string weaponName = "증기 리볼버";
    [SerializeField] private string armSkillName = "기계 왼팔 전격";
    [SerializeField] private float damage = 30f;
    [SerializeField] private float range = 80f;
    [SerializeField] private float fireInterval = 0.18f;
    [SerializeField] private int magazineSize = 12;
    [SerializeField] private float reloadTime = 1.35f;
    [SerializeField] private LayerMask hitMask = ~0;

    [Header("피드백")]
    [SerializeField] private float recoilPitch = 4.5f;
    [SerializeField] private float recoilReturnSpeed = 16f;
    [SerializeField] private float shotShakeDuration = 0.12f;
    [SerializeField] private float shotShakeAmount = 0.055f;
    [SerializeField] private float hitFeedbackTime = 0.2f;
    [SerializeField] private float killFeedbackTime = 0.65f;

    private int ammoInMagazine;
    private float fireTimer;
    private float reloadTimer;
    private float feedbackTimer;
    private float hitFeedbackTimer;
    private float killFeedbackTimer;
    private float emptyFeedbackTimer;
    private float recoilOffset;
    private float shakeTimer;
    private Vector3 cameraBaseLocalPosition;
    private Quaternion cameraBaseLocalRotation;
    private Transform viewWeapon;
    private Vector3 viewWeaponBasePosition;
    private Quaternion viewWeaponBaseRotation;

    public int AmmoInMagazine => ammoInMagazine;

    public int MagazineSize => magazineSize;

    public string WeaponName => weaponName;

    public string ArmSkillName => armSkillName;

    public bool IsReloading => reloadTimer > 0f;

    public float ReloadProgress01 => reloadTime <= 0f ? 1f : 1f - Mathf.Clamp01(reloadTimer / reloadTime);

    public float Feedback01 => Mathf.Clamp01(feedbackTimer / 0.08f);

    public float HitFeedback01 => Mathf.Clamp01(hitFeedbackTimer / hitFeedbackTime);

    public float KillFeedback01 => Mathf.Clamp01(killFeedbackTimer / killFeedbackTime);

    public float EmptyFeedback01 => Mathf.Clamp01(emptyFeedbackTimer / 0.25f);

    public bool LastShotHit { get; private set; }

    public string LastFeedbackMessage { get; private set; } = "대기";

    private void Awake()
    {
        if (aimCamera == null)
        {
            aimCamera = GetComponent<Camera>();
        }

        ammoInMagazine = magazineSize;
    }

    private void Start()
    {
        CacheFeedbackTransforms();
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        feedbackTimer -= Time.deltaTime;
        hitFeedbackTimer -= Time.deltaTime;
        killFeedbackTimer -= Time.deltaTime;
        emptyFeedbackTimer -= Time.deltaTime;
        shakeTimer -= Time.deltaTime;

        UpdateRecoilFeedback();

        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0f)
            {
                ammoInMagazine = magazineSize;
                LastFeedbackMessage = "재장전 완료";
            }

            return;
        }

        if (WasReloadPressed() && ammoInMagazine < magazineSize)
        {
            StartReload();
            return;
        }

        if (IsFireHeld())
        {
            TryFire();
        }
    }

    public void Initialize(Camera camera)
    {
        aimCamera = camera;
        CacheFeedbackTransforms();
    }

    private void TryFire()
    {
        if (fireTimer > 0f)
        {
            return;
        }

        if (ammoInMagazine <= 0)
        {
            emptyFeedbackTimer = 0.25f;
            LastFeedbackMessage = "탄약 없음";
            StartReload();
            return;
        }

        fireTimer = fireInterval;
        feedbackTimer = 0.08f;
        shakeTimer = shotShakeDuration;
        recoilOffset += recoilPitch;
        ammoInMagazine--;
        LastShotHit = false;
        LastFeedbackMessage = "발사";

        var ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out var hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            SpawnMuzzleFlash();
            return;
        }

        var damageable = hit.collider.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            LastShotHit = true;
            hitFeedbackTimer = hitFeedbackTime;
            LastFeedbackMessage = damageable.IsAlive ? "명중" : "처치";

            if (!damageable.IsAlive)
            {
                killFeedbackTimer = killFeedbackTime;
            }
        }

        SpawnMuzzleFlash();
        SpawnHitMarker(hit.point, hit.normal);
    }

    private void StartReload()
    {
        if (reloadTimer > 0f)
        {
            return;
        }

        reloadTimer = reloadTime;
        LastFeedbackMessage = "재장전";
    }

    private void CacheFeedbackTransforms()
    {
        if (aimCamera == null)
        {
            return;
        }

        cameraBaseLocalPosition = aimCamera.transform.localPosition;
        cameraBaseLocalRotation = aimCamera.transform.localRotation;

        viewWeapon = aimCamera.transform.Find("SteamRevolver");
        if (viewWeapon == null)
        {
            return;
        }

        viewWeaponBasePosition = viewWeapon.localPosition;
        viewWeaponBaseRotation = viewWeapon.localRotation;
    }

    private void UpdateRecoilFeedback()
    {
        if (aimCamera == null)
        {
            return;
        }

        recoilOffset = Mathf.MoveTowards(recoilOffset, 0f, recoilReturnSpeed * Time.deltaTime);
        var shake = shakeTimer > 0f ? Random.insideUnitSphere * shotShakeAmount * (shakeTimer / shotShakeDuration) : Vector3.zero;
        aimCamera.transform.localPosition = cameraBaseLocalPosition + shake;
        aimCamera.transform.localRotation = cameraBaseLocalRotation * Quaternion.Euler(-recoilOffset, 0f, 0f);

        if (viewWeapon == null)
        {
            return;
        }

        var kick = Feedback01;
        viewWeapon.localPosition = viewWeaponBasePosition + new Vector3(0f, -0.02f * kick, -0.12f * kick);
        viewWeapon.localRotation = viewWeaponBaseRotation * Quaternion.Euler(-8f * kick, 0f, 0f);
    }

    private void SpawnMuzzleFlash()
    {
        if (aimCamera == null)
        {
            return;
        }

        var flash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flash.name = "MuzzleFlash";
        flash.transform.SetParent(aimCamera.transform, false);
        flash.transform.localPosition = new Vector3(0.34f, -0.28f, 1.18f);
        flash.transform.localScale = Vector3.one * 0.22f;

        var renderer = flash.GetComponent<Renderer>();
        renderer.sharedMaterial = CreateTransientMaterial("MuzzleFlash_Mat", new Color(1f, 0.54f, 0.08f));

        Destroy(flash.GetComponent<Collider>());
        Destroy(flash, 0.055f);
    }

    private static bool IsFireHeld()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.leftButton.isPressed;
#else
        return Input.GetMouseButton(0);
#endif
    }

    private static bool WasReloadPressed()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.R);
#endif
    }

    private static void SpawnHitMarker(Vector3 position, Vector3 normal)
    {
        var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.name = "HitMarker";
        marker.transform.position = position + normal * 0.03f;
        marker.transform.localScale = Vector3.one * 0.13f;
        marker.GetComponent<Renderer>().sharedMaterial = CreateTransientMaterial("HitSpark_Mat", new Color(1f, 0.86f, 0.16f));
        Destroy(marker.GetComponent<Collider>());
        Destroy(marker, 0.25f);
    }

    private static Material CreateTransientMaterial(string name, Color color)
    {
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.name = name;
        material.color = color;
        return material;
    }
}
