using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterIntegration : SocialMediaButton {

    //2018-12-03: copied from an answer by Justin Markwell: https://gamedev.stackexchange.com/questions/115131/how-to-share-unity-high-scores-on-social-media-facebook-google-and-twitter
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWEET_LANGUAGE = "en";
    public static string descriptionParam;
    public string appStoreLink = "http://www.YOUROWNAPPLINK.com";

    public override void postToSocialMedia()
    {

        string nameParameter = "YOUR AWESOME GAME MESSAGE!";//this is limited in text length 
        Application.OpenURL(TWITTER_ADDRESS +
           "?text=" + WWW.EscapeURL(nameParameter + "\n" + descriptionParam + "\n" + "Get the Game:\n" + appStoreLink));
    }
}
