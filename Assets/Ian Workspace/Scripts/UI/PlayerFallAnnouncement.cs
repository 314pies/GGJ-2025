using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TextAnimation;
using TMPro;
using UnityEngine;

public class PlayerFallAnnouncement : MonoBehaviour
{
    private string[] announcements = new string[] {
        "Waves whisper as the sea swallows a fading cry.",
        "Fingers reach, but water pulls them into silence.",
        "A final breath vanishes beneath the rising tide.",
        "The ocean sighs, cradling another forgotten soul.",
        "Sunlight fades as the deep claims its own.",
        "Bubbles rise, but no voice follows.",
        "The sea takes without promise of return.",
        "A splash, a struggle, then only ripples remain.",
        "Darkness unfolds where the water closes in.",
        "The tide erases, but the depths remember."
    };

    public Typewriter playerFallTypeWrite;
    public TMP_Text announcementText;

    public Typewriter playerLeftTypeWriter;
    public TMP_Text playerLeftText;

    [Button]
    public void TestAnnouncePlayerFall()
    {
        AnnouncePlayerFall(2);
    }

    private void Start()
    {
        playerFallTypeWrite.Hide();
        playerLeftTypeWriter.Hide();
    }

    public void AnnouncePlayerFall(int leftPlayerCount)
    {
        playerFallTypeWrite.Hide();
        playerLeftTypeWriter.Hide();

        StartCoroutine(playAnnouncement(leftPlayerCount));
    }


    IEnumerator playAnnouncement(int leftPlayerCount)
    {
        announcementText.text = announcements[Random.Range(0, announcements.Length)];
        playerFallTypeWrite.StartTypewriter();
        yield return new WaitForSeconds(3.5f);

        playerLeftTypeWriter.StartTypewriter();
        playerLeftText.text = leftPlayerCount + " Players Left...";

        yield return new WaitForSeconds(5.0f);
        playerFallTypeWrite.Hide();
        playerLeftTypeWriter.Hide();
    }
}
