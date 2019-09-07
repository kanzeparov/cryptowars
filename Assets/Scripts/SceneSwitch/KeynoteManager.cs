using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeynoteManager : MonoSingleton<KeynoteManager>
{
    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void CreatePatent()
    {
        var hash = "";
        hash = DNAEditor.Instance.CulcVirusHash();
        ContractManager.Instance.CallContract(ContractName.CreatePatent, hash, 10);
    }

    public void Rent()
    {
        
    }

    public void GetInsurance()
    {
        
    }

    public void Win()
    {
        
    }
}
