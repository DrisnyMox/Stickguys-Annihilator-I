using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DamageDetect : MonoBehaviour
{
	[Header("Оптимизация коллизий")]
	public bool autoDisableCollision = false;
	public float disableCollisionTime = 60f;
	private bool isMeatCollisionDisabled = false;
	private Collider2D myCollider;

	Color oldColor;
	public GameObject blood;
	public int numberLevel;

	Transform ui;
	Transform PoolBlood;
	float timePause = 0;
	AudioSource source;
	SpriteRenderer sprRender;
	SpriteRenderer sprBone;
	bool corutinePlayed = false;
	StickmanAudioController stickAudioController;
	Text txtExp;
	public static GameObject car;
	Rigidbody2D rgdbody;
	int distanceOfDisabled;
	DetectRagdoll detectRagdoll;
	BoneColor boneColor;
	public static Fridge fridge;
	public static bool beFridge;

	bool isAudioSource;
	bool isStickmanAudionController;

	void Awake()
	{
		beFridge = false;
		myCollider = GetComponent<Collider2D>();
	}

	// Use this for initialization
	IEnumerator Start()
	{
		// Безопасный поиск UI и текста опыта
		GameObject uiObject = GameObject.Find("UI");
		if (uiObject != null)
		{
			ui = uiObject.transform;
			if (ui.childCount > 0)
			{
				txtExp = ui.GetChild(ui.childCount - 1).GetComponent<Text>();
			}
        }
        else
        {
			autoDisableCollision = true;

		}

		// Запускаем таймер отключения коллизии, если галочка стоит
		if (autoDisableCollision)
		{
			StartCoroutine(DisableCollisionTimer());
		}

		GameObject poolBloodObj = GameObject.Find("Pool Blood");
		if (poolBloodObj != null)
		{
			PoolBlood = poolBloodObj.transform;
		}

		if (GetComponent<SpriteRenderer>())
		{
			oldColor = GetComponent<SpriteRenderer>().color;
		}

		

		source = GetComponent<AudioSource>();
		if (source)
			isAudioSource = true;

		sprRender = GetComponent<SpriteRenderer>();
		stickAudioController = GetComponentInParent<StickmanAudioController>();
		if (stickAudioController)
			isStickmanAudionController = true;

		if (transform.parent != null && transform.parent.name.IndexOf("Xymus") >= 0)
		{
			HingeJoint2D hj2d = GetComponent<HingeJoint2D>();
			if (hj2d && transform.parent.name.IndexOf("Pizdos") <= 0)
			{
				hj2d.breakForce = (UnityEngine.Random.Range(3f, 8f) * 10000f);
			}
		}
		rgdbody = GetComponent<Rigidbody2D>();

		ComponentMenager compManager = GetComponentInParent<ComponentMenager>();
		if (compManager != null)
		{
			distanceOfDisabled = compManager.distanceOfDisabled;
		}

		detectRagdoll = GetComponent<DetectRagdoll>();

		if (transform.childCount > 0)
		{
			Transform firstChild = transform.GetChild(0);
			SpriteRenderer childSpr = firstChild.GetComponent<SpriteRenderer>();
			if (childSpr != null)
			{
				childSpr.color = new Color(1, 1, 1);
			}
			firstChild.gameObject.SetActive(false);
		}

		Transform boneTransform = transform.Find("Bone");
		if (boneTransform != null)
		{
			sprBone = boneTransform.GetComponent<SpriteRenderer>();
		}

		boneColor = HUD.sBoneColor;
		yield return new WaitForSeconds(5.8f);
	}

	IEnumerator DisableCollisionTimer()
	{
		yield return new WaitForSeconds(disableCollisionTime);
		isMeatCollisionDisabled = true;
	}

	// Общий метод для проверки и игнорирования коллизии с мясом
	bool CheckAndIgnoreMeatCollision(Collision2D col)
	{
		if (myCollider == null || col.collider == null) return false;

		bool isMeat = false;

		if (col.transform.CompareTag("MEAT") || col.transform.CompareTag("BOBER"))
		{
			isMeat = true;
		}
		else if (col.transform.parent != null && (col.transform.parent.CompareTag("MEAT") || col.transform.parent.CompareTag("BOBER")))
		{

			isMeat = true;
			
		}

		if (isMeat)
		{
			Physics2D.IgnoreCollision(myCollider, col.collider, true);
			return true;
		}

		return false;
	}

	// Если объекты уже лежали друг на друге, когда сработал таймер, этот метод их разлепит
	void OnCollisionStay2D(Collision2D col)
	{
		if (isMeatCollisionDisabled)
		{
			CheckAndIgnoreMeatCollision(col);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		// Проверяем отключение коллизии ПЕРЕД просчетом урона
		if (isMeatCollisionDisabled)
		{
			if (CheckAndIgnoreMeatCollision(col))
			{
				return; // Прерываем логику урона, крови и звуков, так как коллизия игнорируется
			}
		}

		float forceCollision = Mathf.Abs(col.relativeVelocity.x) + Mathf.Abs(col.relativeVelocity.y);
		if (forceCollision > 10)
		{
			if (col.transform.CompareTag("CAR") || col.transform.CompareTag("BOBER") || col.transform.CompareTag("BULLET"))
			{
				sprRender.color = new Color(0.58f, 0.153f, 0.153f);
				if (!corutinePlayed)
				{
					StartCoroutine(ColorDown());
				}
				if (BloodActivator.enable)// TODO
					SpawnBlood();

				if (isAudioSource)
				{
					if (source.time <= 0.001f)
					{
						source.Play();
					}
				}

				if (isStickmanAudionController)
					stickAudioController.PlayShotScream();

				if (ui)
				{
					if (numberLevel <= 1)
					{
						Levels.currentExperience[numberLevel] += 10;
					}
					else
					{
						Levels.currentExperience[numberLevel] += (int)(10 * (numberLevel / 1.3f));
					}
				}

				// Безопасное обновление текста
				if (txtExp != null)
				{
					txtExp.text = $"{Settings.lng.txt_ExpShort} {Levels.currentExperience[numberLevel]}";
				}

				if (sprBone && boneColor != null)
				{
					Color c = boneColor.currentColor;
					c.a = Mathf.Clamp((forceCollision / Time.timeScale / 300f), 0f, 0.88f);
					sprBone.color = c;
				}
			}
			if (col.transform.parent && col.transform.parent.CompareTag("MEAT"))
			{
				if (transform.parent != null && transform.parent.name != col.transform.parent.name)
				{
					sprRender.color = new Color(0.54f, 0.164f, 0.164f);
					if (!corutinePlayed)
					{
						StartCoroutine(ColorDown());
					}
					if (BloodActivator.enable)// TODO
						SpawnBlood();

					if (ui)
					{
						if (numberLevel <= 1)
						{
							Levels.currentExperience[numberLevel] += 5;
						}
						else
						{
							Levels.currentExperience[numberLevel] += (int)(5 * (numberLevel / 1.3f));
						}
					}
				}
			}
		}

		if (beFridge && fridge != null && fridge.stickmansIced != null)
		{
			Fridge.StickForFreeze stick = fridge.stickmansIced.Find(s => s.stickman == transform.parent);
			if (stick != null && stick.iced == true)
			{
				HingeJoint2D hinge = GetComponent<HingeJoint2D>();
				if (hinge != null)
				{
					rgdbody.gravityScale = 1;
					rgdbody.drag = 0;
					Destroy(hinge);

					if (IceBreak.breaks != null)
					{
						foreach (IceBreak ib in IceBreak.breaks)
						{
							if (!ib.use)
							{
								ib.SetBreak(transform);
								break;
							}
						}
					}
				}
			}
		}
	}

	void SpawnBlood()
	{
		if (Blood.GetBlood(transform))
		{
			timePause = Time.deltaTime;
		}
	}

	IEnumerator ColorDown()
	{
		corutinePlayed = true;
		yield return new WaitForSeconds(1.8f);
		if (sprRender != null)
		{
			sprRender.color = oldColor;
		}
		if (sprBone != null)
		{
			sprBone.color = new Color(1, 1, 1, 0);
		}
		corutinePlayed = false;
	}
}