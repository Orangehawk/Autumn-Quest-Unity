using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
	//Field to set how many clothes to pick from
	public InputField inpNumClothes;

	//Settings fields
	public InputField inpSettingNumClothes;
	public InputField inpSettingColours;

	//Text to display generated result
	public Text txtColour;
	public Text txtNumber;
	public Image txtColourBackground;

	//UI Panels
	public GameObject mainPanel;
	public GameObject settingsPanel;
	public GameObject dialogBox;

	//Storage variables
	public uint numberOfClothes; //Stores how many clothes the user has set to pick from
	public uint settingNumOfClothes; //Stores the setting for the default number of clothes
	public string settingColours; //Stores the setting for the colour list

	//Defaults for resetting
	private uint defaultNumOfClothes = 10;
	private string defaultColours = "Red,Yellow,Blue,Orange,Green,Violet,Pink,White,Black,Gray";

	//Generator
	Generator gen;

	//Debug
	[SerializeField]
	Text debugText;

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

	public void InitSettings()
	{
		LoadSettings();
		inpSettingNumClothes.text = settingNumOfClothes.ToString();
		inpSettingColours.text = settingColours;
	}

	public void ReloadGenerator()
	{
		inpNumClothes.text = PlayerPrefs.GetInt("NumClothes").ToString();
		gen.LoadColours(PlayerPrefs.GetString("Colours").Split(','));
	}

	public void Generate()
	{
		gen.SetMaxClothes((int)numberOfClothes); //Update the maxclothes value in the generator from our numeric counter
		string colourName = gen.GetRandomColour(); //Get a random colour
		txtColour.text = colourName;
		System.Drawing.Color colour = System.Drawing.Color.FromName(txtColour.text);
		txtColourBackground.color = new Color(colour.R / 255f, colour.G / 255f, colour.B / 255f);
		txtNumber.text = $"#{gen.GetRandomNumber()}";
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
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetInt("NumClothes", int.Parse(inpSettingNumClothes.text));
		inpSettingColours.text.Replace(" ", "");
		PlayerPrefs.SetString("Colours", inpSettingColours.text);
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
		inpSettingColours.text = "Red,Yellow,Blue,Orange,Green,Violet,Pink,White,Black,Gray";
	}

	public void DialogueAnswer(bool answer)
	{
		if (answer == true)
		{
			dialogBox.SetActive(false);
			settingsPanel.SetActive(true);
			inpSettingColours.text = "Red,Yellow,Blue,Orange,Green,Violet,Pink,White,Black,Gray";
		}
		else
		{
			dialogBox.SetActive(false);
			settingsPanel.SetActive(true);
		}
	}
}



/*public class ColourFinder
{
	public struct RGB
	{
		public RGB(int r, int g, int b)
		{
			red = r;
			green = g;
			blue = b;
		}

		public int red;
		public int green;
		public int blue;
	}

	static Dictionary<string, RGB> colors;
	
	public static void LoadColorsFromResources(string path)
	{
		colors = new Dictionary<string, RGB>();

		TextAsset file = Resources.Load<TextAsset>("Colours");

		string[] list = file.text.Split('\n');

		foreach (string line in list)
		{
			string[] items = line.Split(',');
			colors.Add(items[0].ToLower(), new RGB(int.Parse(items[1]), int.Parse(items[2]), int.Parse(items[3])));
		}
	}

	public static RGB RGBFromName(string name)
	{
		if(colors.ContainsKey(name.ToLower()))
		{
			return colors[name.ToLower()];
		}
		else
		{
			return new RGB(255, 255, 255);
		}
	}

	public static UnityEngine.Color ColorFromName(string name)
	{
		if (colors.ContainsKey(name.ToLower()))
		{
			RGB rgb = colors[name.ToLower()];
			return new UnityEngine.Color(rgb.red, rgb.green, rgb.blue);
		}
		else
		{
			return new UnityEngine.Color(255, 255, 255);
		}
	}
}*/