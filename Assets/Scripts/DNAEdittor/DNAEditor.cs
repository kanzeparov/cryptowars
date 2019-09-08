using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Globalization;

[Serializable]
public class CustomResponse
{
    public int id;
}

[Serializable]
public class CustomResponse2
{
    public float[] coordsx = new float[12];
}

public class DNAEditor : MonoSingleton<DNAEditor>
{
    public Text buttonText;
    public Transform DNANodePrefub;
    public ConnectionLine ConnectionLinePrefub;

    public Text HealthText;
    public Text HealSpeedText;
    public Text HealthTextAdd;
    public Text HealSpeedTextSpeed;

    public Button getPaternBtn;

    public ConnectionLine ConnectionLine;

    private EditMode editMode = EditMode.CreateDNA;
    private List<Transform> dnaNodeList;

    private double HPVirus;
    private double healSpeed;

    private DNANode firstConNode;
    private DNANode secondConNode;

    private bool isClicking = false;
    private Vector2 mousePosition;

    private int connectionsCount = 0;

    public void Start()
    {
        dnaNodeList = new List<Transform>();
        getPaternBtn.onClick.AddListener(delegate { StartCoroutine(createNewVirusAsync()); } );

        DNANode.onMouseOver.AddListener(MouseOverNode);
        DNANode.onMouseExit.AddListener(MouseExitNode);
    }

    public void Update()
    {
        getMosePosition();

        if (!isClicking && Input.GetMouseButton(0))
        {
            isClicking = true;
            MouseButtonDown();
        }
        else if (isClicking && !Input.GetMouseButton(0))
        {
            isClicking = false;
            MouseButtonUp();
        }

        if (isClicking)
        {
            if (mousePosition != Vector2.zero)
            {
                if (editMode == EditMode.CreateConnection)
                {
                    setConnection();
                }
            }
        }
    }

    public void ChangeEditMode()
    {
        switch (this.editMode)
        {
            case EditMode.CreateConnection:
                this.editMode = EditMode.CreateDNA;
                buttonText.text = "Connection";
                break;

            case EditMode.CreateDNA:
                this.editMode = EditMode.CreateConnection;
                buttonText.text = "Dots";
                break;
        }
    }

    public void SaveVirus()
    {
        PlayerPrefs.SetFloat("HPVirus", (float)this.HPVirus);
        PlayerPrefs.SetFloat("healSpeed", (float)this.healSpeed);
    }

    private void setConnection()
    {
        if (editMode == EditMode.CreateDNA)
            return;
        if (firstConNode == null)
        {
            if (ConnectionLine != null)
                ConnectionLine.gameObject.SetActive(false);
            return;
        }

        if (ConnectionLine != null)
            ConnectionLine.gameObject.SetActive(true);

        if (secondConNode == null)
        {
            if (ConnectionLine != null)
                ConnectionLine.SetPosition(firstConNode.transform.position, mousePosition);
            return;
        }
        if (ConnectionLine != null)
            ConnectionLine.SetPosition(firstConNode.transform.position, secondConNode.transform.position);
    }

    private void updateVirusMetaInfo()
    {
        float sum = (float)ConvertHexStringToSum(CulcVirusHash());
        float HPAdd = 0;
        float healSpeedAdd = 0;
        if (sum > 2300)
        {
            HPAdd = Mathf.Round(((sum / 4028) * 100));
            healSpeedAdd = Mathf.Round(((sum) / 4028) * 10);
        }


        this.HPVirus = Mathf.Round((float)(dnaNodeList.Count * 10.3 + 1.5 - connectionsCount * 2 + HPAdd)); 
        this.healSpeed = Mathf.Round((float)(connectionsCount * 0.6 + 0.1 + healSpeedAdd));

        HealthText.text = "HEALTH " + this.HPVirus;
        HealSpeedText.text = "HEAL SPEED " + this.healSpeed;


        if (HPAdd > 1)
            HealthTextAdd.text = "+" + HPAdd;
        else
            HealthTextAdd.text = "";

        if (healSpeedAdd > 0)
            HealSpeedTextSpeed.text = "+" + healSpeedAdd;
        else
            HealSpeedTextSpeed.text = "";
    }

    public string CulcVirusHash()
    {
        var firstNodePos = this.dnaNodeList[0].position;

        var dnaPosList = this.dnaNodeList.OrderBy(node => node.position.x).ThenBy(node => node.position.y)
                          .Select(node => (node.position.x - firstNodePos.x) + " " + (node.position.y - firstNodePos.y)).ToArray();
        string nodesHash = getHashString(string.Join(" ", dnaPosList));
        return nodesHash;
    }

    private void getMosePosition()
    {
        Vector2 pos = Vector2.zero;
        if (Input.GetMouseButton(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(camRay, out hit, 2f))
            {
                pos = hit.point;
            }
        }
        mousePosition = pos;
    }

    private void MouseOverNode(DNANode node)
    {
        if (editMode == EditMode.CreateDNA)
            return;

        if (!isClicking)
            return;

        if (firstConNode == null)
        {
            firstConNode = node;
            if (ConnectionLine == null)
                ConnectionLine = Instantiate(ConnectionLinePrefub);
            return;
        }

        if (secondConNode == null && firstConNode != node)
        {
            secondConNode = node;
        }

    }

    private void MouseExitNode(DNANode node)
    {
        if (editMode == EditMode.CreateDNA)
            return;

        if (secondConNode != null && node == secondConNode)
            secondConNode = null;
    }

    private void MouseButtonUp()
    {
        if (editMode == EditMode.CreateDNA)
            return;

        if (firstConNode != null && secondConNode != null)
        {
            ConnectionLine = null;
            connectionsCount++;
            updateVirusMetaInfo();
        }
        else
        {
            if (ConnectionLine != null)
            {
                Destroy(ConnectionLine.gameObject);
                ConnectionLine = null;
            }
        }
        firstConNode = null;
        secondConNode = null;
    }

    private void MouseButtonDown()
    {
        if (editMode == EditMode.CreateConnection)
            return;

        if (mousePosition != Vector2.zero)
        {
            var newDNANode = Instantiate(DNANodePrefub, mousePosition, Quaternion.identity);
            this.dnaNodeList.Add(newDNANode);
            //Debug.Log(newDNANode.position.x);
            //Debug.Log(newDNANode.position.y);
            updateVirusMetaInfo();
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

    private static byte[] ConvertHexStringToByteArray(string hexString)
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

    private static int ConvertHexStringToSum(string hexString)
    {
        if (hexString.Length % 2 != 0)
        {
            throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
        }

        int sum = 0;
        for (int i = 0; i < hexString.Length/2; i++)
        {
            string byteValue = hexString.Substring(i * 2, 2);
            sum += byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        return sum;
    }

    private IEnumerator createNewVirusAsync()
    {
        //Proyecto26.RestClient.Get("http://ya.ru").Then(response => {
        //    UnityEditor.EditorUtility.DisplayDialog("Response", response.Text, "Ok");
        //});

        float[] coords = new float[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int my_count = 0;
        foreach (Transform cur_dna_node in this.dnaNodeList)
        {
            coords[my_count] = cur_dna_node.position.x;
            my_count++;
        }

        var usersRoute = "http://localhost:8080/";
        var testRoute = "https://jsonplaceholder.typicode.com/posts";
        var newUser = 1;
        //Proyecto26.RestClient.Post<CustomResponse>(usersRoute, newUser).Then(customResponse => {
        //    UnityEditor.EditorUtility.DisplayDialog("JSON", JsonUtility.ToJson(customResponse, true), "Ok");
        //});
        Debug.Log(Proyecto26.RestClient.Post<CustomResponse>(usersRoute, this.dnaNodeList.Count).Then(customResponse => {
            UnityEditor.EditorUtility.DisplayDialog("JSON", JsonUtility.ToJson(customResponse, true), "Ok");
        }));
        Debug.Log(Proyecto26.RestClient.Post<CustomResponse2>(usersRoute, coords).Then(customResponse => {
        UnityEditor.EditorUtility.DisplayDialog("JSON", JsonUtility.ToJson(customResponse, true), "Ok");
        }));
        //Debug.Log(Proyecto26.RestClient.Post<CustomResponse>(testRoute, newUser).Then(customResponse => {
        //    UnityEditor.EditorUtility.DisplayDialog("JSON", JsonUtility.ToJson(customResponse, true), "Ok");
        //}));
        //Debug.Log(Proyecto26.RestClient.Get("http://ya.ru"));


        //Proyecto26.RestClient.Post("https://jsonplaceholder.typicode.com/posts", newPost).Then(response => {
        //    UnityEditor.EditorUtility.DisplayDialog("Status", response.StatusCode.ToString(), "Ok");
        //});

        var address = ConvertHexStringToByteArray("cf2cc8dcffc74cfe0a079fdbb16c0d6f78290527bfbcef935553d50746ecc00f");

        var newVirusContract = new NewVirusRequest(address);

        string hash = CulcVirusHash();

        float rentCost = 10;
        if (rentCost >= 0)
        {
            print("start add patent");
            yield return newVirusContract.NewVirus(hash, Mathf.RoundToInt(rentCost));

            printProcessResult(newVirusContract);
            print(rentCost);
         }
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
}


public enum EditMode { CreateDNA, CreateConnection };