using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] Characters;
    public GameObject[] Infos;
    public int Number;

    void Start()
    {
        UpdateSelection();
    }

    public void ChangeCharacter(int Num)
    {
        // Atualiza o número do personagem
        Number += Num;
        if (Number >= Characters.Length)
            Number = 0;
        if (Number < 0)
            Number = Characters.Length - 1;

        UpdateSelection();
    }

    void UpdateSelection()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            bool isActive = (i == Number);

            // Ativa/desativa personagens
            if (Characters[i] != null)
                Characters[i].SetActive(isActive);

            // Ativa/desativa infos
            if (Infos[i] != null)
                Infos[i].SetActive(isActive);

            // Toca animação Idle apenas no personagem ativo
            if (isActive && Characters[i] != null)
            {
                Animator anim = Characters[i].GetComponent<Animator>();
                if (anim != null)
                    anim.Play("Idle");
            }
        }
    }

}
