using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator
{
	List<string> colours;
	int maxClothes;

	public Generator()
	{
		colours = new List<string>();
		maxClothes = 0;
	}

	public void LoadColours(string[] colourList)
	{
		colours.Clear();
		colours.AddRange(colourList);
	}

	public void LoadColours(List<string> colourList)
	{
		colours = colourList;
	}

	public void SetMaxClothes(int numOfClothes)
	{
		maxClothes = numOfClothes;
	}

	public string GetRandomColour()
	{
		if (colours.Count > 1)
		{
			return colours[Random.Range(0, colours.Count - 1)];
		}
		else if (colours.Count == 1)
		{
			return colours[0];
		}
		else
		{
			return "Error";
		}
	}

	public int GetRandomNumber()
	{
		if (maxClothes > 0)
		{
			return Random.Range(1, maxClothes);
		}
		else
		{
			return 0;
		}
	}
}

public class Manager : MonoBehaviour
{
	//Main UI Elements
	public InputField inpNumClothes;
	public Button inpGenerate;

	//Settings fields
	public InputField inpSettingNumClothes;
	public InputField inpSettingColours;
	public Toggle inpSettingSkipSpin;

	//Text to display generated result
	public Text txtColour;
	public Text txtNumber;
	public Image txtColourBackground;

	//UI Panels
	public GameObject mainPanel;
	public GameObject settingsPanel;
	public GameObject dialogBox;

	//Storage variables
	private uint numberOfClothes; //Stores how many clothes the user has set to pick from
	private uint settingNumOfClothes; //Stores the setting for the default number of clothes
	private string settingColours; //Stores the setting for the colour list
	private bool settingSkipSpin; //Store the setting for whether to skip the spin

	//Defaults for resetting
	uint defaultNumOfClothes = 10;
	string defaultColours = "Red,Yellow,Blue,Orange,Green,Violet,Pink,White,Black,Gray";

	//Generator
	Generator gen;

	//Carbie Spinner
	public GameObject wheel;
	public ColourChanger mainCarbie;
	[SerializeField]
	float spinTime = 5f;

	// Start is called before the first frame update
	void Start()
	{
		InitMainPanel();
		gen = new Generator();
		gen.LoadColours(settingColours.Split(','));
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Quit()
	{
		Application.Quit();
	}

	void InitMainPanel()
	{
		LoadSettings();
		inpNumClothes.text = settingNumOfClothes.ToString();
	}

	//Loads all the setting values into the setting UI elements
	public void InitSettings()
	{
		LoadSettings();
		inpSettingNumClothes.text = settingNumOfClothes.ToString();
		inpSettingColours.text = settingColours;
		inpSettingSkipSpin.isOn = settingSkipSpin;
	}

	//Reloads the values the 
	public void ReloadGenerator()
	{
		inpNumClothes.text = PlayerPrefs.GetInt("NumClothes").ToString();
		gen.LoadColours(PlayerPrefs.GetString("Colours").Split(','));
	}

	Color ContrastColor(System.Drawing.Color color)
	{
		//Counting the perceptive luminance - human eye favors green color
		double luminance = (color.R * 0.299f + color.G * 0.587f + color.B * 0.114f) / 256f;

		if (luminance > 0.55)
		{
			return Color.black;
		}
		else
		{
			return Color.white;
		}
	}

	public void Generate()
	{
		gen.SetMaxClothes((int)numberOfClothes); //Update the maxclothes value in the generator from our numeric counter
		string colourName = gen.GetRandomColour(); //Get a random colour
		System.Drawing.Color colour = System.Drawing.Color.FromName(colourName);

		if (settingSkipSpin) //Setting for skip
		{
			txtNumber.text = $"#{gen.GetRandomNumber()}";
			txtColour.text = colourName;
			txtColourBackground.color = new Color(colour.R / 255f, colour.G / 255f, colour.B / 255f);
			txtColour.color = ContrastColor(colour);
			mainCarbie.ChangeColor(colour);
		}
		else
		{
			IEnumerator coroutine;

			coroutine = Coroutine_Generate(colourName, colour);
			StartCoroutine(coroutine);

			coroutine = Coroutine_SpinWheel(spinTime);
			StartCoroutine(coroutine);

			coroutine = Coroutine_ColourCarbie(colour);
			StartCoroutine(coroutine);
		}
	}

	IEnumerator Coroutine_Generate(string colourName, System.Drawing.Color colour)
	{
		txtNumber.text = "Generating...";
		txtColour.text = "Generating...";
		txtColourBackground.color = Color.white;
		txtColour.color = new Color(0.1960784f, 0.1960784f, 0.1960784f); //Default colour used for text
		inpGenerate.enabled = false; //Disable the generate button while the wheel spins

		yield return new WaitForSeconds(spinTime);

		txtNumber.text = $"#{gen.GetRandomNumber()}"; //Get a random number for the clothes #
		txtColour.text = colourName;
		txtColourBackground.color = new Color(colour.R / 255f, colour.G / 255f, colour.B / 255f);
		txtColour.color = ContrastColor(colour);
		inpGenerate.enabled = true; //Enable the generate button again
	}

	IEnumerator Coroutine_SpinWheel(float duration)
	{
		float startRotation = transform.eulerAngles.y;
		float endRotation = startRotation + 360.0f;
		float t = 0.0f;
		while (t < duration)
		{
			t += Time.deltaTime;
			float yRotation = Mathf.SmoothStep(startRotation, endRotation, t / duration) % 360.0f;
			wheel.transform.eulerAngles = new Vector3(wheel.transform.eulerAngles.x, yRotation, wheel.transform.eulerAngles.z);
			yield return null;
		}
	}

	IEnumerator Coroutine_ColourCarbie(System.Drawing.Color colour)
	{
		yield return new WaitForSeconds(spinTime / 2);
		mainCarbie.ChangeColor(colour);
	}

	public void LoadSettings()
	{
		if (PlayerPrefs.HasKey("NumClothes"))
			settingNumOfClothes = (uint)PlayerPrefs.GetInt("NumClothes");
		else
			settingNumOfClothes = defaultNumOfClothes;

		if (PlayerPrefs.HasKey("Colours"))
			settingColours = PlayerPrefs.GetString("Colours");
		else
			settingColours = defaultColours;

		if (PlayerPrefs.HasKey("SkipSpin"))
			settingSkipSpin = PlayerPrefs.GetString("SkipSpin").Contains("True");
		else
			settingSkipSpin = false;
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetInt("NumClothes", int.Parse(inpSettingNumClothes.text));
		inpSettingColours.text.Replace(" ", "");
		PlayerPrefs.SetString("Colours", inpSettingColours.text);
		PlayerPrefs.SetString("SkipSpin", inpSettingSkipSpin.isOn.ToString());
		settingSkipSpin = inpSettingSkipSpin.isOn;
		PlayerPrefs.Save();
		CloseSettings();
		ReloadGenerator();
	}

	public void OpenSettings()
	{
		mainPanel.SetActive(false);
		settingsPanel.SetActive(true);
		InitSettings();
	}

	public void CloseSettings()
	{
		mainPanel.SetActive(true);
		settingsPanel.SetActive(false);
		dialogBox.SetActive(false);
	}

	public void ResetColours()
	{
		dialogBox.SetActive(true);
		settingsPanel.SetActive(false);
		inpSettingColours.text = defaultColours;
	}

	public void DialogueAnswer(bool answer)
	{
		if (answer == true)
		{
			dialogBox.SetActive(false);
			settingsPanel.SetActive(true);
			inpSettingColours.text = defaultColours;
		}
		else
		{
			dialogBox.SetActive(false);
			settingsPanel.SetActive(true);
		}
	}
}