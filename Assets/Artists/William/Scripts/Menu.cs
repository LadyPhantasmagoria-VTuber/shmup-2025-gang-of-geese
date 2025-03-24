using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene(0); // Load Game

    }

    //public void OnMenuButton()
   // {
       // SceneManager.LoadScene(); // Load Menu

  //  }

    //public void OnControlsButton()
    //{
    //    SceneManager.LoadScene(); // Load Controls
    //}

   public void OnQuitButton()
   {
      Application.Quit(); // Quit the game
   }
}

//For additional scenes to switch between, copy paste a section and make sure the number in brackets alignes with the number in your scene manager (Look 
//in the file -> Build profile -> Scene list to find out the corresponding number for the scenes) 