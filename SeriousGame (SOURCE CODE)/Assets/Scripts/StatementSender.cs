using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TinCan;
using TinCan.LRSResponses;

public class StatementSender : MonoBehaviour
{
    public string _actor;
    public string _verb;
    public string _definition;
    public int _value = 0;

    private RemoteLRS lrs;

    private void Start()
    {
        lrs = new RemoteLRS(
            "https://watershedlrs.com/api/organizations/17062/lrs/",
            "7710434b961ef2",
            "8bae7d1288e970"
        );
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SendStatement();
        }
    }

    public void SendStatement()
    {
        //Build the Actor
        Agent actor = new Agent();
        actor.mbox = "mailto:" + _actor.Replace(" ", "") + "@email.com";
        actor.name = _actor;

        //Build the Verb
        Verb verb = new Verb();
        verb.id = new Uri("https://www.example.com/" + _verb.Replace(" ", ""));
        verb.display = new LanguageMap();
        verb.display.Add("en-US", _verb);

        //Build the Activity
        Activity activity = new Activity();
        activity.id = new Uri("https://www.example.com/" + _definition.Replace(" ", "")).ToString();

        //Build the Activity Definition
        ActivityDefinition activityDefinition = new ActivityDefinition();
        activityDefinition.description = new LanguageMap();
        activityDefinition.name = new LanguageMap();
        activityDefinition.name.Add("en-US", _definition);
        activity.definition = activityDefinition;

        Result result = new Result();
        Score score = new Score();

        score.raw = _value;
        result.score = score;

        //Build the full Statement
        Statement statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;
        statement.result = result;

        //Send the Statement
        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);

        if(lrsResponse.success)
        {
            Debug.Log("Statement Success: " + lrsResponse.content.id);
        }
        else
        {
            Debug.Log("Statement Failed: " + lrsResponse.errMsg);
        }
    }
}
