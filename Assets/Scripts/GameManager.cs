using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelUpAnimation levelUpAnimation;
    [SerializeField] private TextMeshProUGUI levelTMP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkIfFoesDefeated();
    }

    private void checkIfFoesDefeated() {
        if (GameStateManager.Instance.LevelData.RemainingFoes <= 0) {
            GameStateManager.Instance.LevelData.AdvanceToNextLevel();
            levelUpAnimation.LevelUp(GameStateManager.Instance.LevelData.ActiveLevel);
        }
    }

    private void AnimateLevelUpTextWobbly() {
        levelTMP.ForceMeshUpdate();
        TMP_TextInfo textInfo = levelTMP.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            int[] dirs = new int[]{-1,};
            for (int j = 0; j < 4; j++)
            {
                Vector3 orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * .01f) * 10f, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            levelTMP.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
