using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ComidaData", menuName = "Comida/Configuração")]
public class comidaConfiig : ScriptableObject
{
    public string nomeComida;
    public int cliquesParaComer = 1;
    public float valorFome = 0f;
    public float valorMaxFome = 0f;
    public bool temPoder = false;      
}
 
