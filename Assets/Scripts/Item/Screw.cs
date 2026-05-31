using PrimeTween;
using UnityEngine;

public enum ScrewStatus
{
    Screwed,
    ScrewTransition,
    Unscrewed
}
public class Screw : MonoBehaviour
{
    [SerializeField] private SpriteRenderer screwSpriteRenderer;
    private float screwDuration;
    private float currentDuration;
    private bool isScrewing = false;
    public ScrewStatus screwStatus;
    private bool isUnscrewing = false;
    private Tween screwRotationTween;
    private Tween screwScaleTween;
    private void Update()
    {
        if (isScrewing == true)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= screwDuration)
            {
                OnComplete(true, ScrewStatus.Screwed);
            }
        }
        if (isUnscrewing == true)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= screwDuration)
            {
                OnComplete(false, ScrewStatus.Unscrewed);
            }
        }
    }
    public void Initialize(float screwDuration)
    {
        this.screwDuration = screwDuration;
    }
    private void StartUnscrew()
    {
        screwRotationTween = Tween.EulerAngles(transform, new Vector3(0f, 0f, 360f), Vector3.zero, 0.8f, Easing.Standard(Ease.Linear), -1);
        screwScaleTween = Tween.Scale(transform, new Vector3(1.3f, 1.3f, 1.3f), screwDuration, Easing.Standard(Ease.Linear));
        screwRotationTween.isPaused = false;
        screwScaleTween.isPaused = false;
        isUnscrewing = true;
        screwStatus = ScrewStatus.ScrewTransition;
    }
    private void StartScrew()
    {
        screwSpriteRenderer.enabled = true;
        screwRotationTween = Tween.EulerAngles(transform, Vector3.zero, new Vector3(0f, 0f, 360f), 0.8f, Easing.Standard(Ease.Linear), -1);
        screwScaleTween = Tween.Scale(transform, new Vector3(1, 1, 1), screwDuration, Easing.Standard(Ease.Linear));
        screwRotationTween.isPaused = false;
        screwScaleTween.isPaused = false;
        isScrewing = true;
        screwStatus = ScrewStatus.ScrewTransition;
    }
    private void OnComplete(bool enableSprite, ScrewStatus newStatus)
    {
        screwStatus = newStatus;
        isScrewing = false;
        isUnscrewing = false;
        screwRotationTween.Complete();
        screwScaleTween.Complete();
        currentDuration = 0f;
        screwSpriteRenderer.enabled = enableSprite;
    }
    public void StartScrewPerform(bool isUnscrew = true)
    {
        if (isUnscrew == false)
        {
            StartScrew();
        }
        else
        {
            StartUnscrew();
        }
    }
    public void PauseScrew()
    {
        if(isScrewing == true)
        {
            isScrewing = false;
            screwStatus = ScrewStatus.Unscrewed;
        }
        if(isUnscrewing == true)
        {
            isUnscrewing = false;
            screwStatus = ScrewStatus.Screwed;
        }
        screwRotationTween.isPaused = true;
        screwScaleTween.isPaused = true;
    }
}
