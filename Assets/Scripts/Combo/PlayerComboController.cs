using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboController : MonoBehaviour
{
    public Combo[] combos;

    private Animator animator;
    private PlayerMovimentation playerMovimentation;

    private bool startCombo = false;
    private float comboTimer;
    private Hit currentHit, nextHit;
    private bool canHit = true;
    private bool resetCombo;

    public List<string> currentCombo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovimentation = GetComponent<PlayerMovimentation>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        for (int i = 0; i < combos.Length; i++)
        {
            if (currentCombo.Count >= combos[i].hits.Length)
            {
                continue;
            }

            if (Input.GetButtonDown(combos[i].hits[currentCombo.Count].inputButton))
            {
                if (currentCombo.Count == 0)
                {
                    PlayHit(combos[i].hits[currentCombo.Count]);
                    playerMovimentation.DisableRun();
                    break;
                }

                bool comboMatch = false;

                for (int j = 0; j < currentCombo.Count; j++)
                {
                    if (currentCombo[j] != combos[i].hits[j].inputButton)
                    {
                        comboMatch = false;
                        break;
                    }

                    comboMatch = true;
                }

                if (comboMatch && canHit)
                {
                    nextHit = combos[i].hits[currentCombo.Count];
                    canHit = false;
                    break;
                }
            }
        }

        if (startCombo)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= currentHit.animationTime && !canHit)
            {
                PlayHit(nextHit);
            }

            if (comboTimer >= currentHit.resetTime)
            {
                ResetCombo();
            }
        }
    }

    void PlayHit(Hit hit)
    {
        comboTimer = 0;
        animator.Play(hit.animationName);
        startCombo = true;
        currentCombo.Add(hit.inputButton);
        currentHit = hit;
        canHit = true;
    }

    void ResetCombo()
    {
        playerMovimentation.EnableRun();
        startCombo = false;
        comboTimer = 0;
        canHit = true;
        currentCombo.Clear();
        animator.Rebind();
    }
}
