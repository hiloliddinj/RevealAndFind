using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> buttonObjectsList;
    [SerializeField] List<GameObject> indicatorsList;

    [SerializeField] Sprite disabledButtonImage;

    

    [SerializeField] Sprite errorImage;
    [SerializeField] Sprite successImage, defaultHeart;
    [SerializeField] Sprite initialButtonSprite;

    [SerializeField] GameObject youWin, youLoose, startPanel, instractionsPanel;

    [SerializeField] AudioSource startGamePressedAudio;

    private List<int> prizesList = new() {15, 15, 15, 25, 25, 25, 5, 5, 5};
    private List<int> randomPrizeList;


    private readonly List<int> foundPrizeList = new();
    int counter;

    int valueToFind = 0;
    int prizeCounter = 0;

    void Start()
    {
        startPanel.SetActive(true);
        counter = 0;
        RandomizePrize();
    }

    void RandomizePrize()
    {
        randomPrizeList = ShuflleList(prizesList, 9);

        for (int i = 0; i < buttonObjectsList.Count; i++)
        {
            buttonObjectsList[i].GetComponent<ButtonSc>().prize = randomPrizeList[i];
        }
    }

    public void ButtonPressed(int index)
    {
        Debug.Log("Pressed index is: " + index);
        ButtonOperations(index);
    }

    private void ButtonOperations(int index)
    {
        if (counter < 5)
        {
            buttonObjectsList[index].GetComponent<AudioSource>().Play();

            buttonObjectsList[index].GetComponent<Button>().interactable = false;

            int foundPrize = buttonObjectsList[index].GetComponent<ButtonSc>().prize;
            buttonObjectsList[index].GetComponentInChildren<TextMeshProUGUI>().text = "$" + foundPrize.ToString();

            buttonObjectsList[index].GetComponent<Image>().sprite = disabledButtonImage;

            if (counter == 0)
            {
                valueToFind = foundPrize;
                indicatorsList[counter].GetComponent<Image>().sprite = successImage;
                prizeCounter++;
            } else
            {
                if (foundPrize == valueToFind)
                {
                    indicatorsList[counter].GetComponent<Image>().sprite = successImage;
                    prizeCounter++;
                } else
                {
                    indicatorsList[counter].GetComponent<Image>().sprite = errorImage;
                }
            }
            

            foundPrizeList.Add(foundPrize);

            counter++;
            Debug.Log("Counter: " + counter);

            if (counter == 5)
            {
                IndicatorOperations();
                foreach (GameObject gO in buttonObjectsList)
                {
                    Button button = gO.GetComponent<Button>();
                    if (button.interactable)
                    {
                        button.enabled = false;
                    }

                }
            }
        }
    }

    private void IndicatorOperations()
    {
        Debug.Log("prizeCounter: " + prizeCounter);
        if (prizeCounter == 3)
        {
            youWin.SetActive(true);
            //youWin.GetComponent<AudioSource>().Play();
        } else
        {
            youLoose.SetActive(true);
            //youLoose.GetComponent<AudioSource>().Play();

        }
    }

    public void PlayAgain()
    {
        RandomizePrize();
        startGamePressedAudio.Play();
        youWin.SetActive(false);
        youLoose.SetActive(false);

        foreach (GameObject gO in buttonObjectsList)
        {
            Button myButton = gO.GetComponent<Button>();
            myButton.interactable = true;
            myButton.enabled = true;
            gO.GetComponent<Image>().sprite = initialButtonSprite;
            gO.GetComponentInChildren<TextMeshProUGUI>().text = "";
            counter = 0;
            prizeCounter = 0;
        }

        foreach(GameObject gO in indicatorsList)
        {
            gO.GetComponent<Image>().sprite = defaultHeart;
        }

    }

    public void StartGame()
    {
        startGamePressedAudio.Play();
        instractionsPanel.SetActive(false);
        RandomizePrize();
    }

    public void ShowInstractionPanel()
    {
        startPanel.SetActive(false);
        instractionsPanel.SetActive(true);
    }



    private List<T> ShuflleList<T>(List<T> inputList, int count) {
        List<T> outputList = new();
        for (int i = 0; i < count; i++) {
            int index = UnityEngine.Random.Range(0, inputList.Count);
            outputList.Add(inputList[index]);
        }
        return outputList;
    }
}
