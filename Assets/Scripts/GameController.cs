using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform imageTransform;

    public List<Sprite> imagesList = new List<Sprite>();
    public List<Sprite> imagesList1 = new List<Sprite>();
    public GameObject image;
    public Sprite[] openedImages;
    public List<int> listNumbers = new List<int>();


    [SerializeField] private Transform puzzleFields;

    [SerializeField] private GameObject images;


    //  Shuffle the list
    private List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = Random.Range(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

    public void getRandomNumber(Sprite[] openedImages)
    {
        int number;
        for (int i = 0; i < 5; i++)
        {
            number = Random.Range(1, openedImages.Length);
            if (!listNumbers.Contains(number))
            {
                listNumbers.Add(number);
            }
        }

        setImages(listNumbers);
    }

    public void setImages(List<int> listNumbers)
    {
        Sprite imageSprite = null;
        for (int i = 0; i < listNumbers.Count; i++)
        {
            int arrayIndex = listNumbers[i];
            imageSprite = openedImages[arrayIndex];
            imagesList.Add(imageSprite);
        }

        for (int i = 0; i < 5; i++)
        {
            image = Instantiate(images);
            image.gameObject.GetComponent<Image>().sprite = imagesList[i];
            image.transform.SetParent(puzzleFields);
        }

        setImageBlack(listNumbers);
    }

    public void setImageBlack(List<int> listNumbers)
    {
        imagesList1 = ShuffleList(imagesList);
        for (int i = 0; i < listNumbers.Count; i++)
        {
            image = Instantiate(images);
            image.gameObject.GetComponent<Image>().sprite = imagesList1[i];
            image.gameObject.GetComponent<Image>().color = Color.black;
            image.transform.SetParent(puzzleFields);
        }
    }

    void Start()
    {
        getRandomNumber(openedImages);
    }


    float tempZAxis;
    public SpriteRenderer selection;
    void Update()
    {
        Touch[] touch = Input.touches;
        for (int i = 0; i < touch.Length; i++)
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            switch (touch[i].phase)
            {
                case TouchPhase.Began:
                    if (hit)
                    {
                        selection = hit.transform.gameObject.GetComponent<SpriteRenderer>();
                        if (selection != null)
                        {
                            tempZAxis = selection.transform.position.z;
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    Vector3 tempVec = Camera.main.ScreenToWorldPoint(touch[i].position);
                    tempVec.z = tempZAxis; //Make sure that the z zxis never change
                    if (selection != null)
                    {
                        selection.transform.position = tempVec;
                    }
                    break;
                case TouchPhase.Ended:
                    selection = null;
                    break;
            }

        }
    }
}