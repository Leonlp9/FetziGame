using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static int geld;
    public Text money;
    // Start is called before the first frame update
    void Start()
    {
        geld = PlayerPrefs.GetInt("money", 0);
    }

    // Update is called once per frame
    void Update()
    {
        money.text = PlayerPrefs.GetInt("money", 0).ToString() + " Sui Coins";
    }

    public static void AddMoney()
    {
        geld++;
        PlayerPrefs.SetInt("money", geld);
    }

    public static void AddMoney(int amount)
    {
        geld *= amount;
        PlayerPrefs.SetInt("money", geld);
    }

}
