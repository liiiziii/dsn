using UnityEngine;
using System.Collections;

public class charTalk : MonoBehaviour
{
		public Renderer[] renderObjs;
		public dialoguePopUp _dialogueBubble;
		public PlayerInputController
				inputController;
		BlockInformation _charInfo;
		[SerializeField]
		TextMesh
				_textUI;
		public string[] lines;
		public int triggerTimes = 1;
		[HideInInspector]
		public bool finishedLines = false;

		void Start ()
		{
				_charInfo = this.GetComponent<BlockInformation> ();
		}

		void Update ()
		{
		if (PlayerInputController.playerStatus == 0)
						return;
		if (triggerTimes >= lines.Length)
			finishedLines = true;
				if (_charInfo.enteredControl) {
			changeColor (Color.grey * 0.4f);
			//show/hide text
						if (Input.GetKeyDown (inputController.rightBtn))
								triggerTimes++;
						if (Input.GetKeyDown (inputController.leftBtn) && triggerTimes % lines.Length > 1)
								triggerTimes--;
						if (triggerTimes % lines.Length == 0) 
								_dialogueBubble.enableDialogue = false;
						else {
							_textUI.text = lines [(triggerTimes - 1) % lines.Length].Replace("<br>", "\n");
								_dialogueBubble.enableDialogue = true;
						}
				} else {
			//hide text
						changeColor (Color.white);
						triggerTimes = 1;
						_dialogueBubble.enableDialogue = false;
				}
		}

		void changeColor (Color c)
		{
				foreach (Renderer r in renderObjs)
						r.material.color = c;
		}
}
