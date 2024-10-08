using UnityEngine;

public class WeaponSounds : MonoBehaviour
{
	public AudioClip shoot;

	public AudioClip reload;

	public AudioClip empty;

	public bool isSerialShooting;

	public GameObject bonusPrefab;

	public GameObject bonusBonus;

	public int InitialAmmo = 24;

	public int ammoInClip = 12;

	public int maxAmmo = 84;

	public bool isMelee;

	public bool isZooming;

	public bool isMagic;

	public float fieldOfViewZomm = 75f;

	public float range = 3f;

	public int damage = 50;

	public float speedModifier = 1f;

	public GameObject animationObject;

	public int Probability = 1;

	public Vector2 damageRange = new Vector2(-15f, 15f);

	public Vector3 gunPosition = new Vector3(0.35f, -0.25f, 0.6f);

	public Texture preview;

	public int inAppExtensionModifier = 10;

	public float meleeAngle = 45f;

	public float multiplayerDamage = 1f;

	public float meleeAttackTimeModifier = 0.57f;

	public Texture2D aimTextureV;

	public Texture2D aimTextureH;

	public Vector2 startZone;

	public float tekKoof = 1f;

	public float upKoofFire = 0.5f;

	public float maxKoof = 4f;

	public float downKoofFirst = 0.2f;

	public float downKoof = 0.2f;

	private float animLength;

	private float timeFromFire = 1000f;

	public int MaxAmmoWithRespectToInApp
	{
		get
		{
			return maxAmmo;
		}
	}

	private void Start()
	{
		if (animationObject != null && animationObject.GetComponent<Animation>()["Shoot"] != null)
		{
			animLength = animationObject.GetComponent<Animation>()["Shoot"].length;
		}
	}

	private void Update()
	{
		if (timeFromFire < animLength)
		{
			timeFromFire += Time.deltaTime;
			if (tekKoof > 1f)
			{
				tekKoof -= downKoofFirst * Time.deltaTime / animLength;
			}
			if (tekKoof < 1f)
			{
				tekKoof = 1f;
			}
		}
		else
		{
			if (tekKoof > 1f)
			{
				tekKoof -= downKoof * Time.deltaTime / animLength;
			}
			if (tekKoof < 1f)
			{
				tekKoof = 1f;
			}
		}
	}

	public void fire()
	{
		timeFromFire = 0f;
		tekKoof += upKoofFire + downKoofFirst;
		if (tekKoof > maxKoof + downKoofFirst)
		{
			tekKoof = maxKoof + downKoofFirst;
		}
	}
}
