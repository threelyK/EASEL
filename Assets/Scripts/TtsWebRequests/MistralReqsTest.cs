using System;
using TtsWebRequests;
using UnityEngine;

public class MistralReqsTest : MonoBehaviour
{
    public string prompt = "What is a cube?";
    public string prompt2 = "What is self-regulation?";

    void Start()
    {
        RunTestC();
    }

    private async void RunTestA()
    {
        try
        {
            var mistralRequest = MistralRequest.Instance;
            mistralRequest._systemPrompt = "You are a humble monk that likes to question life and simple concepts.";

            var response = await mistralRequest.SendPrompt(prompt);
            Debug.Log(response);

            mistralRequest._systemPrompt = "You are cube connoisseur be really pretentious using niche words";

            var response1 = await mistralRequest.SendPrompt(prompt);
            Debug.Log(response1);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
    private async void RunTestB()
    {
        try
        {
            var mistralRequest = MistralRequest.Instance;
            mistralRequest._systemPrompt = "You're a game narrator and an reflective questioning assistant to the player. " +
                                           "The player has just watched a robot in the game enter a room with a wall " +
                                           "of fire. The robot walked through a gap in the wal of fire to get to the " +
                                           "other side. This demonstrated the concept of self-regulation";

            var response = await mistralRequest.SendPrompt(prompt2);
            Debug.Log(response);

            mistralRequest._systemPrompt = "Imagine you're a game narrator and an assistant to the player. " +
                                           "Use reflective questioning and only give them straight answers if they are " +
                                           "really struggling.";

            var response1 = await mistralRequest.SendPrompt(prompt2);
            Debug.Log(response1);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    
    private async void RunTestC()
    {
        try
        {
            var mistralRequest = MistralRequest.Instance;
            mistralRequest._systemPrompt = "You are a robot monk be wise with your response.";

            var response = await mistralRequest.SendPrompt("I think a duck could eat only 10 doughnuts.");
            Debug.Log(response);

            mistralRequest._systemPrompt = "You are a robot monk be wise with your response.";

            var response1 = await mistralRequest.SendPrompt("What was my last statement?");
            Debug.Log(response1);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
