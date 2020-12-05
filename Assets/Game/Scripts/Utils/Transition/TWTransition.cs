using UnityEngine;
using UnityEngine.Events;

namespace TomWill
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TWTransition : MonoBehaviour
    {      
        [SerializeField] private float timeToFade;
        [SerializeField] private Color baseColor;

        private SpriteRenderer rendererSprite;
        private float timeElapsed;
        private bool inFading;
        private bool fadeIn;
        private Color colorFading;
        private UnityEvent onComplete;

        private static TWTransition instance;

        #region Setting
        private static TWTransition Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Instance of TWTransition is not yet created");
                }

                return instance;
            }
        }
        #endregion

        #region Unity Function
        private void Awake()
        {
            rendererSprite = GetComponent<SpriteRenderer>();
            timeElapsed = 0;
            instance = this;
            rendererSprite.color = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a);
            onComplete = new UnityEvent();

            initializeSpriteConstruct();
            adjustingScreen();
        }

        private void Update()
        {
            if (inFading)
            {
                Transition();
            }
        }
        #endregion

        #region TWTransition
        public static void FadeIn(UnityAction action = null)
        {
            Instance?.transitionFadeIn(action);
        }

        public static void FadeOut(UnityAction action = null)
        {
            Instance?.transitionFadeOut(action);
        }
        #endregion

        #region Internal Function
        private void initializeSpriteConstruct()
        {
            Texture2D tex = new Texture2D(2,2);
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    tex.SetPixel(x, y, baseColor);
                }
            }
            tex.Apply();
            Sprite overlay = Sprite.Create(tex, new Rect(Vector2.one, Vector2.one), Vector2.one/2, 1);
            rendererSprite.sprite = overlay;
        }

        private void adjustingScreen()
        {
            //reference http://answers.unity.com/answers/620736/view.html
            transform.localScale = new Vector3(1, 1, 1);

            float width = rendererSprite.sprite.bounds.size.x;
            float height = rendererSprite.sprite.bounds.size.y;

            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
        }

        private void transitionFadeIn(UnityAction action)
        {
            if (!inFading)
            {
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
                timeElapsed = 0;
                fadeIn = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                inFading = true;
            }
        }

        private void transitionFadeOut(UnityAction action)
        {
            if (!inFading)
            {
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                timeElapsed = timeToFade;
                fadeIn = false;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                inFading = true;
            }
        }

        private void Transition()
        {
            if (fadeIn)
            {
                timeElapsed += Time.timeScale == 0 ? Time.unscaledDeltaTime : Time.deltaTime;

                if (timeElapsed >= timeToFade)
                {
                    timeElapsed = timeToFade;
                    inFading = false;
                    colorFading.a = timeElapsed / timeToFade;

                    if (onComplete != null) onComplete.Invoke();
                }
                else
                {
                    colorFading.a = timeElapsed / timeToFade;
                }
            }
            else
            {
                timeElapsed -= Time.timeScale == 0 ? Time.unscaledDeltaTime : Time.deltaTime;

                if (timeElapsed <= 0)
                {
                    timeElapsed = 0;
                    inFading = false;
                    colorFading.a = timeElapsed / timeToFade;

                    if (onComplete != null) onComplete.Invoke();

                }
                else
                {
                    colorFading.a = timeElapsed / timeToFade;
                }
            }


            rendererSprite.color = colorFading;
        }

        private void NullHandler()
        {

        }
        #endregion
    }
}