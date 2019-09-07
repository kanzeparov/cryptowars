using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

public class ContractManager : MonoSingleton<ContractManager>
{

    /// <summary>
    /// Win - (byte[]) arg0 - кошелёк победителя, (string) arg1 - хеш вируса
    /// Rent - (string)arg0, (int)arg1 - хеш вируса, токен
    /// CreatePatent - (string)arg0, (int)arg1 - хеш вируса, rent cost
    /// </summary>
    /// <param name="contractName"></param>
    public void CallContract(ContractName contractName, object arg0, object arg1)
    {
        StartCoroutine(callContract(contractName, arg0, arg1));
    }

    public IEnumerator callContract(ContractName contractName, object arg0, object arg1)
    {
        var address = ConvertHexStringToByteArray("cf2cc8dcffc74cfe0a079fdbb16c0d6f78290527bfbcef935553d50746ecc00f");

        switch (contractName)
        {
            case ContractName.Win:
                var winReq = new WinRequest(address);
                yield return winReq.Win((byte[])arg0, (string)arg1);
                printProcessResult(winReq); //log
                break;
            case ContractName.Rent:
                var rentReq = new RentRequest(address);
                yield return rentReq.Rent((string)arg0, (int)arg1);
                printProcessResult(rentReq);
                break;
            case ContractName.CreatePatent:
                var patentReq = new NewVirusRequest(address);
                yield return patentReq.NewVirus((string)arg0, (int)arg1);
                printProcessResult(patentReq);

                break;
        }


    }

    public static byte[] ConvertHexStringToByteArray(string hexString)
    {
        if (hexString.Length % 2 != 0)
        {
            throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
        }

        byte[] HexAsBytes = new byte[hexString.Length / 2];
        for (int index = 0; index < HexAsBytes.Length; index++)
        {
            string byteValue = hexString.Substring(index * 2, 2);
            HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        return HexAsBytes;
    }

    public static string IntArrayAndStringToHash(int[] inputArray, string inputStr)
    {
        List<string> list = new List<string>() { inputStr } ;
        list.AddRange(inputArray.Select(t => t.ToString()));
             
        string inputString = String.Join("", list.ToArray());

        return getHashString(inputString);
    }

    public static string IntArrayToHash(int[] inputArray)
    {
        string inputString = String.Join("", inputArray.Select(t => t.ToString()).ToArray());
        return getHashString(inputString);
    }

    private void printProcessResult<T>(ProgramRequest<T> req)
    {
        if (req.IsError)
        {
            print("error hepens");
            print(req.Error);
        }
        else
        {
            print(req.Result);
        }
    }

    private static string getHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in getHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

    private static byte[] getHash(string inputString)
    {
        HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

}

public enum ContractName { CreatePatent, Rent, Win}


