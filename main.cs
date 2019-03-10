using System;
using System.Security.Cryptography;
using System.Text;

class MainClass {
  public static void Main (string[] args) {
    int option;
    bool cont = true;

    //------- MENU --------//

    Console.WriteLine("This app will take any list of passwords and convert them for you into hashes which will be stored in a returnable rainbow table.");
    Console.WriteLine("Created by: WizKi9d \n");
    while(cont == true) {
    Console.WriteLine("Enter an option: ");
    Console.WriteLine("   1) View hashes and their passwords.");
    Console.WriteLine("   2) Add a list of passwords to create hashes");
    Console.WriteLine("   3) Get password value for hash.");
    Console.WriteLine("/> ");
    option = Convert.ToInt32(Console.ReadLine());

    // Continue loop until said otherwise
      // View the hashes and the passwords they correlate to
      if(option == 1) {
        // Uses function to read through all the tables in the file
        Console.WriteLine("Enter file to view: (or write 'all' for all");
        string fileOption = Console.ReadLine();
        readRainbowTables(fileOption);
      } 
      // Add list of passwords and make a new file that contains hash and password
      else if(option == 2) {
        Console.WriteLine("The program will let you enter your lists after you import them into: home/PasswordLists/... \n");
        Console.WriteLine("Enter the files name: ");
        string enterFile = Console.ReadLine();
        Console.WriteLine("\n");
        makeHashTable(enterFile);
        Console.WriteLine("\n");
      } 
      // Get the value for the entered hash
      else if(option == 3) {
        Console.WriteLine("Enter your hash: ");
        string enterHash = Console.ReadLine();
        Console.WriteLine("\n");
        string realValue = findUnhashedValue(enterHash);
        Console.WriteLine(realValue.Replace(" ", string.Empty));
        Console.WriteLine("\n");
      } else {
        Console.WriteLine("Sorry, that's not an option!");
      }
    }

  }

  public static string MD5Hash(string input)
  {
    StringBuilder hash = new StringBuilder();
    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

    for (int i = 0; i < bytes.Length; i++)
    {
      hash.Append(bytes[i].ToString("x2"));
    }
    return hash.ToString();
  }

  public static void appendToFile(string md5Hash, string actualString) {
    using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@"RainbowTable/rainbow.txt", true))
        {
            file.WriteLine(md5Hash + ", " + actualString);
        }
  }

  public static string readFromFile(string hash) {
    string stringFound;
    string[] lines = System.IO.File.ReadAllLines(@"RainbowTable/rainbow.txt");

    foreach(string line in lines) {
      string[] actualString = line.Split(',');
      if(actualString[0] == hash) {
        stringFound = actualString[1];
        return stringFound.Replace(" ", string.Empty);
      } else {
        continue;
      }
    }
    return("Not Found");

  }

  // Create hash table from password lists given
  public static void makeHashTable(string fileName) {
    string[] lines = System.IO.File.ReadAllLines(@"PasswordLists/" + fileName);
    
    foreach(string line in lines) {
      string getHash = MD5Hash(line);
      using(System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@"RainbowTable/RainbowTwo.txt", true))
          {
            file.WriteLine(getHash + ", " + line);
          }
      Console.WriteLine("Saved: " + getHash + ", " + line + " .... succesfully!");

    }
  }

  public static void readRainbowTables(string option) {
    // Reads over all files
    if(option == "all") {
      // Finds all files in the folder path
      foreach (string file in System.IO.Directory.GetFiles("RainbowTable", "*.txt"))
      {
          string contents = System.IO.File.ReadAllText(file);
          Console.WriteLine(contents);
      }
    } 
    // Reads over given file
    else {
      // Check first if file exists
      if(System.IO.File.Exists("RainbowTables/" + option)) {
        string[] lines = System.IO.File.ReadAllLines(@"RainbowTables/" + option);
        foreach(string line in lines) {
          Console.WriteLine(line);
        }
      } else {
        Console.WriteLine("File doesn't exist!");
      }
    }
  }

  public static string findUnhashedValue(string hashValue) {
    //foreach (string file in System.IO.Directory.GetFiles("RainbowTable", "*.txt"))
    //  {
    //      string contents = System.IO.File.ReadAllText(file);
    //      Console.WriteLine(contents);
    //  }
   
    string[] files = System.IO.Directory.GetFiles("RainbowTable");
    foreach(string file in files) {
      string getFileName = System.IO.Path.GetFileName(file);
      //Console.WriteLine(getFileName);
      
      //Get the file and go over each line
      string[] myFile = System.IO.File.ReadAllLines(@"RainbowTable/" + getFileName);
      foreach(string line in myFile) {
        //Split each line where the comma is
        string[] splitFile = line.Split(',');
        //See if the given value matches the value at pointer [1]
        if(hashValue == splitFile[0]) {
          //Return the real password
          return(splitFile[1]);
        } else {
          continue;
        }
        //If the hash isnt in the table do this
        return("No hashed sum found.");
      }
    }
    return("Error");
  }
}