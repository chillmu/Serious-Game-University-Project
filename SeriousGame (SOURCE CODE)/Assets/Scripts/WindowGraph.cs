using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite; //Sprite used to display a point/dot/circle on the graph
    private RectTransform background; //The rect transform of the entire canvas object
    private RectTransform graphContainer; //The rect transform of the graph container gameobject
    private RectTransform labelTemplateX; //The rect transform of the x-axis label
    private RectTransform labelTemplateY; //The rect transform of the y-axis label
    private RectTransform dashTemplateX; //The rect transform of the x-axis line 
    private RectTransform dashTemplateY; //The rect transform of the y-axis line

    [SerializeField] private GameObject closeMenuButton;
    GameObject thisMenu;

    List<Response> correctResponses;
    List<Response> incorrectResponses;

    Dictionary<string, int> correctDateTimeValues;
    Dictionary<string, int> incorrectDateTimeValues;

    List<int> correctValues;
    List<int> incorrectValues;

    DateTime minDate;
    DateTime maxDate;

    int maxResult;
    int minResult;

    List<GameObject> gameObjectList;

    public GameObject graphTitle;

    /// <summary>
    /// Awake, called before everything, used to get reference of the gameobjects needed to build the graph
    /// </summary>
    private void Awake()
    {
        thisMenu = this.gameObject.transform.parent.gameObject;
        thisMenu.transform.localScale = new Vector3(1, 1, 1);

        closeMenuButton.GetComponent<Button>().onClick.AddListener(delegate { CloseMenu(); });

        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        background = graphContainer.Find("Background").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();
    }

    void DestroyGameObjects()
    {
        foreach(GameObject o in gameObjectList)
        {
            Destroy(o.gameObject);
        }
    }

    void CloseMenu()
    {
        DestroyGameObjects();
        thisMenu.SetActive(false);
    }

    public void ProcessResponses(List<Response> responses)
    {
        gameObjectList = new List<GameObject>();

        if(responses.Count <= 0)
        {
            if(graphContainer.gameObject.GetComponent<Text>())
            {
                graphContainer.gameObject.GetComponent<Text>().text = "Not Enough Data, you need to make multiple responses related to this character over a couple of days";
                graphContainer.gameObject.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                graphContainer.gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                graphContainer.gameObject.GetComponent<Text>().resizeTextForBestFit = true;
            }
            else
            {
                graphContainer.gameObject.AddComponent<Text>().text = "Not Enough Data, you need to make multiple responses related to this character over a couple of days";
                graphContainer.gameObject.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                graphContainer.gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                graphContainer.gameObject.GetComponent<Text>().resizeTextForBestFit = true;
            }
            
            return;
        }
        else
        {
            if(graphContainer.gameObject.TryGetComponent<Text>(out Text text))
            {
                Destroy(graphContainer.gameObject.GetComponent<Text>());
            }
        }


        List<Response> correctResponses = new List<Response>();
        List<Response> incorrectResponses = new List<Response>();

        bool firstPass = true;

        foreach(Response response in responses)
        {

            if (firstPass == true)
            {
                minDate = DateTime.Parse(response.GetResponseDate());
                maxDate = DateTime.Parse(response.GetResponseDate());

                firstPass = false;
            }

            if(response.GetResponseOutcome() == "Correct")
            {
                correctResponses.Add(response);
            }
            else
            {
                incorrectResponses.Add(response);
            }

            minResult = DateTime.Compare(minDate, DateTime.Parse(response.GetResponseDate()));
            maxResult = DateTime.Compare(maxDate, DateTime.Parse(response.GetResponseDate()));

            //determine the earliest date amoungst all responses
            if(minResult > 0)
            {
                minDate = DateTime.Parse(response.GetResponseDate());
            }

            //determine the latest date amoungst all responses
            if(maxResult < 0)
            {
                maxDate = DateTime.Parse(response.GetResponseDate());
            }
        }

        //number of days between the earliest and latest date
        int days = (int)(maxDate - minDate).TotalDays;
        days = Mathf.Abs(days);

        //days was like 26 or something
        //so the first keyvaluepair will need to return 0 for xPosition
        //we can get the date of the keyvaluepair and we have the earliest date and latest date
        // we can do the above equation to determine difference in days from the minumum date

        //Sort the lists so they are ordered by datetime
        correctResponses = correctResponses.OrderBy(o => o.responseDate).ToList();
        incorrectResponses = incorrectResponses.OrderBy(o => o.responseDate).ToList();

        //Dictionaries to store the totals for each date for each list of responses
        correctDateTimeValues = new Dictionary<string, int>();
        incorrectDateTimeValues = new Dictionary<string, int>();

        //For each date between the min and max date, loop through each response and build the dictionary
        for(DateTime date = minDate; date <= maxDate; date = date.AddDays(1))
        {
            correctDateTimeValues.Add(date.ToString("yyyy-MM-dd"), 0);

            foreach(Response r in correctResponses)
            {
                if(DateTime.Parse(r.responseDate).ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                {
                    correctDateTimeValues[DateTime.Parse(r.responseDate).ToString("yyyy-MM-dd")]++;
                }
            }
        }       
        
        //For each date between the min and max date, loop through each response and build the dictionary
        for(DateTime date = minDate; date <= maxDate; date = date.AddDays(1))
        {
            incorrectDateTimeValues.Add(date.ToString("yyyy-MM-dd"), 0);

            foreach(Response r in incorrectResponses)
            {
                if(DateTime.Parse(r.responseDate).ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd"))
                {
                    incorrectDateTimeValues[DateTime.Parse(r.responseDate).ToString("yyyy-MM-dd")]++;
                }
            }
        }

        correctValues = new List<int>();
        foreach(KeyValuePair<string, int> pair in correctDateTimeValues)
        {
            correctValues.Add(pair.Value);
        }

        incorrectValues = new List<int>();
        foreach(KeyValuePair<string, int> pair in incorrectDateTimeValues)
        {
            incorrectValues.Add(pair.Value);
        }

        BuildGraph(correctDateTimeValues, incorrectDateTimeValues, (int i) => "Day " + (i + 1));
    }

    private void BuildGraph(Dictionary<string, int> correctValues, Dictionary<string, int> incorrectValues, Func<int, string> getAxisLabelX = null)
    {
        //return the string a-axis label
        if(getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int i) { return i.ToString(); };
        }

        DestroyGameObjects();

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float points = Mathf.Max(correctValues.Count, incorrectValues.Count);
        float yMax = 0f;

        foreach(KeyValuePair<string, int> pair in correctValues)
        {
            if(pair.Value > yMax)
            {
                yMax = pair.Value;
            }
        }

        foreach(KeyValuePair<string, int> pair in incorrectValues)
        {
            if(pair.Value > yMax)
            {
                yMax = pair.Value;
            }
        }

        yMax = yMax * 1.2f; //Adding a little headway to the height of the graph

        float xSize = graphWidth / (points - 1); //best distance between x-axis coordinates

        GameObject lastCorrectCircleGameObject = null; //Reference to the last circle gameobject

        foreach(KeyValuePair<string, int> pair in correctValues)
        {
            //Figure out how many days from the minimum date this current date is
            DateTime date = DateTime.Parse(pair.Key);
            int days = (int)date.Subtract(minDate.AddDays(-1)).TotalDays;

            float xPosition = days * xSize;
            float yPosition = (pair.Value / yMax) * graphHeight;

            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), Color.green);
            gameObjectList.Add(circleGameObject);

            if(lastCorrectCircleGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastCorrectCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, Color.green);
                gameObjectList.Add(dotConnectionGameObject);
            }

            lastCorrectCircleGameObject = circleGameObject;

            if(correctValues.Count > incorrectValues.Count)
            {
                RectTransform labelX = Instantiate(labelTemplateX);
                labelX.SetParent(graphContainer);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -25f);
                labelX.GetComponent<Text>().text = date.ToString("yyyy-MM-dd");//getAxisLabelX(days);
                labelX.transform.Rotate(new Vector3(0, 0, 90));
                gameObjectList.Add(labelX.gameObject);

                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(xPosition, 0f);
                gameObjectList.Add(dashX.gameObject);
            }

            if(correctValues.Count == incorrectValues.Count)
            {
                RectTransform labelX = Instantiate(labelTemplateX);
                labelX.SetParent(graphContainer);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -25f);
                labelX.GetComponent<Text>().text = date.ToString("yyyy-MM-dd"); //getAxisLabelX(days);
                labelX.transform.Rotate(new Vector3(0, 0, 90));
                gameObjectList.Add(labelX.gameObject);

                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(xPosition, 0f);
                gameObjectList.Add(dashX.gameObject);
            }
        }

        GameObject lastIncorrectCircleGameObject = null; //Reference to the last circle gameobject

        //Loop the incorrect value list and create all of the dots and lines, along with the x-axis labels and x-axis lines if this is the larger list
        foreach(KeyValuePair<string, int> pair in incorrectValues)
        {
            //Figure out how many days from the minimum date this current date is
            DateTime date = DateTime.Parse(pair.Key);
            int days = (int)date.Subtract(minDate.AddDays(-1)).TotalDays;

            float xPosition = days * xSize;
            float yPosition = (pair.Value / yMax) * graphHeight;

            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), Color.red);
            gameObjectList.Add(circleGameObject);

            if(lastIncorrectCircleGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(lastIncorrectCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, Color.red);
                gameObjectList.Add(dotConnectionGameObject);
            }

            lastIncorrectCircleGameObject = circleGameObject;

            if(incorrectValues.Count > correctValues.Count)
            {
                RectTransform labelX = Instantiate(labelTemplateX);
                labelX.SetParent(graphContainer);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -25f);
                labelX.GetComponent<Text>().text = date.ToString("yyyy-MM-dd"); //getAxisLabelX(days);
                labelX.transform.Rotate(new Vector3(0, 0, 90));
                gameObjectList.Add(labelX.gameObject);

                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(xPosition, 0f);
                gameObjectList.Add(dashX.gameObject);
            }
        }

        //the number in which the y-axis values scale by
        int seperatorCount = 10;

        for (int i = 0; i <= seperatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / seperatorCount;
            labelY.anchoredPosition = new Vector2(-25f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMax).ToString();
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(0f, normalizedValue * graphHeight);

            gameObjectList.Add(dashY.gameObject);
        }
    }

    /// <summary>
    /// Used to draw a point on the graph
    /// </summary>
    /// <param name="anchoredPosition">The position the dot needs to be placed</param>
    /// <returns>The circle gameobject to be used as reference to draw a line between that and the next point</returns>
    private GameObject CreateCircle(Vector2 anchoredPosition, Color color)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        color.a = 0.75f;
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    /// <summary>
    /// Creates a line between two dots on the graph 
    /// </summary>
    /// <param name="dotPositionA">The position of the first dot</param>
    /// <param name="dotPositionB">The position of the second dot</param>
    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color color)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        color.a = 0.5f;
        gameObject.GetComponent<Image>().color = color;
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
        return gameObject;
    }
}
