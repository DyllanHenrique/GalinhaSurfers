using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] Characters;
    public int Number;

    void Start()
    {
        // Garante que só o personagem selecionado esteja ativo
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i] != null)
            {
                Characters[i].SetActive(i == Number); // Ativa apenas o selecionado

                Animator anim = Characters[i].GetComponent<Animator>();
                if (anim != null && i == Number)
                {
                    anim.Play("Idle"); // inicia Idle apenas no personagem ativo
                }
            }
        }
    }

    public void ChangeCharacter(int Num)
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i] != null)
                Characters[i].SetActive(false);
        }

        Number += Num;

        if (Number >= Characters.Length)
            Number = 0;
        if (Number < 0)
            Number = Characters.Length - 1;

        if (Characters[Number] != null)
        {
            Characters[Number].SetActive(true);
            Animator anim = Characters[Number].GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("Idle");
            }
        }
    }

}
