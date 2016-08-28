using UnityEngine;
using System.Collections;

public class charDial : MonoBehaviour
{
				
		public Renderer[] renderObjs;
		public dialoguePopUp[] _dialogueBubbles;
		public PlayerInputController
				inputController;
		BlockInformation _charInfo;
		[SerializeField]
		TextMesh[]
				_textUI;
		public string[] linesL;
		public string[] linesR;
		public int[] triggerTimes;
		public bool[] finishedLines;

		void Start ()
		{
				_charInfo = this.GetComponent<BlockInformation> ();
		}

		void Update ()
		{
				if (PlayerInputController.playerStatus == 0)
						changeLines (0, linesL);
				else
						changeLines (1, linesR);
		}

		void changeLines(int i, string[] l ){
				if (triggerTimes[i] >= l.Length)
						finishedLines[i] = true;
				if (_charInfo.enteredControl) {
						changeColor (Color.grey * 0.4f);
						//show/hide text
						if (Input.GetKeyDown (inputController.rightBtn))
								triggerTimes[i]++;
						if (Input.GetKeyDown (inputController.leftBtn) && triggerTimes[i] % l.Length > 1)
								triggerTimes[i]--;
						if (triggerTimes[i] % l.Length == 0)
								_dialogueBubbles[i].enableDialogue = false;
						else {
								int id = (triggerTimes [i] - 1) % l.Length;
								_textUI[i].text = l [id].Replace ("<br>", "\n");
								_dialogueBubbles[i].enableDialogue = true;
						}
				} else {
						//hide text
						changeColor (Color.white);
						triggerTimes[i] = 1;
						_dialogueBubbles[i].enableDialogue = false;
				}
		}


		void changeColor (Color c)
		{
				foreach (Renderer r in renderObjs)
						r.material.color = c;
		}
}