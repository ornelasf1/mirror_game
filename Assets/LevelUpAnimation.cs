using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpAnimation : MonoBehaviour
{
    private RectTransform levelTextRect;
    private TextMeshProUGUI levelTMP;
    private const float startAnimationDurationInSeconds = 0.3f;
    private const float endAnimationDurationInSeconds = 0.3f;
    private float StartAnimation;
    private float EndAnimation;
    private Vector3 originalScale = new Vector3(1,1,0);
    private bool isAnimationComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        levelTextRect = GetComponent<RectTransform>();
        levelTMP = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimationComplete) {
            AnimateText();
        }
    }

    public void LevelUp(int newLevel) {
        levelTMP.text = $"Level {newLevel}";
        StartAnimation = startAnimationDurationInSeconds;
        EndAnimation = endAnimationDurationInSeconds;
        isAnimationComplete = false;
    }

    private void AnimateText() {
        if (StartAnimation > 0f) {
            StartAnimation -= Time.deltaTime;
            levelTextRect.localScale += new Vector3(4,4,0) * Time.deltaTime;
        } else if (EndAnimation > 0f) {
            EndAnimation -= Time.deltaTime;
            levelTextRect.localScale += new Vector3(-4,-4,0) * Time.deltaTime;
        } else {
            isAnimationComplete = true;
            levelTextRect.localScale = originalScale;
        }
    }
}
