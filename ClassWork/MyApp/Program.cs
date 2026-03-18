// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;
using System.IO;
using MyApp;
using MyApp.ConsoleApp;
using System.Collections;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

// class Program
// {
//     static void Main()
//     {
//         string connectionString =
//             "Server=localhost,1433;Database=master;User Id=SA;Password=Diksha@123;TrustServerCertificate=True;";

//         using (SqlConnection conn = new SqlConnection(connectionString))
//         {
//             conn.Open();
//             Console.WriteLine("Connected to SQL Server inside Docker!");

//             string sql = "SELECT @@VERSION"; // sample query

//             using (SqlCommand cmd = new SqlCommand(sql, conn))
//             {
//                 var result = cmd.ExecuteScalar();
//                 Console.WriteLine("SQL Server Version: " + result);
//             }
//         }
//     }
// }



// {
//     static void Main(string[] args)
//     {
//         Totals t = new Totals();
//         t.Sum(10, 5);
//     }
// }

// public class Totals
// {
//     public void Sum(int x, int y)
//     {
//         Console.WriteLine(x + y);
//     }
// }

// class Program
// {
//     static void Main(string[] args)
//     {
//         Console.Write("Enter your age: ");
//         byte age = Convert.ToByte(Console.ReadLine());
//         string message = (age < 3) ? "Hey small guy" : (age > 3 && age < 10) ? "Hey kid" : (age < 18 && age > 10) ? "Hey teen" : (age > 18) ? "Hey big guy" : "invalid age";
//         Console.WriteLine(message);
//     }
// }

// class Program{
//     static void Main(string[] args){
//         string article = " Don't talk to me you idiot.";
//         bool check = article.Contains("idiot");
//         if(check){
//             Console.WriteLine("Rejected");
//         }
//         else{
//             Console.WriteLine("Accepted");
//         }
//     }
// }


// ArithematicOperations ao = new ArithematicOperations();
// Console.Write("Please enter the first value: ");
// int x = int.Parse(Console.ReadLine());
// int[] result = new int[3];
// result = ao.Calculate(x);
// for (int i = 0; i < result.Length; i++)
// {
//     Console.WriteLine(result[i]);
// }


// Console.Write("Please enter your age: ");
// try
// {
//     byte age = byte.Parse(Console.ReadLine());
//     if (age < 10) Console.WriteLine("Hey kid");
//     else Console.WriteLine("Hey big guy");
// }
// catch
// {
//     Console.WriteLine("Please enter a valid age");
// }     


// Area a = new Area(10);
// Console.WriteLine(a.CalculateArea());
// Area b = new Area(10, 20);
// Console.WriteLine(b.CalculateArea());

//string manipulation

// string str = "Hello, My name is, Diksha,";
// string str1 = "  Hello  ";
// Console.WriteLine("the length of String is: " + str.Length);
// Console.WriteLine("the string converted to caps is: " + str.ToUpper());
// Console.WriteLine("the string converted to lowercase is: " + str.ToLower());
// Console.WriteLine("the substring of 3 char starting from l is: "+str.Substring(3, 3));
// Console.WriteLine("Does the string contain 'name'? " + str.Contains("name"));
// Console.WriteLine("What is the index of D? " + str.IndexOf("D"));
// Console.WriteLine("Replace H with F: " + str.Replace("H", "F"));
// Console.WriteLine("Trimming spaces: " + str1.Trim());
// string[] parts = str.Split(',');
// Console.Write("The ',' split of str is: ");
// for (int i = 0; i < parts.Length; i++)
// {
//     Console.Write(" " + parts[i]);
// }
// Console.WriteLine();
// Console.WriteLine("Does the string start with 'He'? " + str.StartsWith("He"));
// Console.WriteLine("Does the string end with 'a,'? " + str.EndsWith("a,"));
// string[] arr = { "a", "b", "c" };
// Console.WriteLine("The string after joining is: " + string.Join("-", arr));
// string name = "Diksha";
// int age = 23;
// Console.WriteLine(string.Format("Name: {0}, Age: {1}", name, age));
// string str3 = "123";
// Console.WriteLine(str3.PadLeft(5, '0'));
// Console.WriteLine(str3.PadRight(5, '0'));
// string a = "Hello";
// string b = "hello";
// Console.WriteLine("Are String a and b equal? " + a.Equals(b));
// Console.WriteLine("The second char in str is: " + str[1]);  
// string str4 = "";
// Console.WriteLine("Is the string null?" + string.IsNullOrEmpty(str4));
// string str5 = "   ";
// Console.WriteLine("Is the string null or has whitespace?" + string.IsNullOrWhiteSpace(str5));
// Console.WriteLine("Are the two strings same?" + string.Compare("Diksha", "Samriddhi"));
// Console.WriteLine("Is Hello same as hello?" + "Hello".CompareTo("hello"));
// Console.WriteLine("Is Hello exactly equal to hello?" + "Hello".Equals("hello"));
// Console.WriteLine("Difference in between D and S" + string.CompareOrdinal("Diksha", "Samriddhi"));
// Console.WriteLine("Removing 'ik' from Diksha" + name.Remove(1, 3));
// Console.WriteLine("Inserting Chaudhary after Diksha" + name.Insert(6, " Chaudhary"));
// Console.WriteLine(string.Concat("Hello", " ", "World"));
// Console.WriteLine("Converting age to string" + age.ToString());
// string str6 = "hello hello hello";
// Console.WriteLine("What is the last index of hello" + str6.LastIndexOf("hello"));
// char[] chars = { 'o', 'e' };
// Console.WriteLine("Index of the characters: " + str1.IndexOfAny(chars));
// Console.WriteLine(str1.LastIndexOfAny(chars));
// char[] arr2 = "DIKSHA".ToCharArray();
// Console.WriteLine(arr2[2]);


// Stack<int> marks = new Stack<int>();
// marks.Push(100);
// marks.Push(99);
// marks.Push(89);

// Console.WriteLine(marks.Peek());
// Console.WriteLine(marks.Pop());
// Console.WriteLine(marks.Peek());

// Queue<string> order = new Queue<string>();

// internal class MyGenericClass<T>
// {
//     public void MyMethod(T z)
//     {
//         if (z.GetType() == typeof(string))
//         {

//         }
//         else
//         {

//         }
//     }
// }

// List<string> countries = new List<string>();
// countries.Add("India");
// countries.Add("USA");
// countries.Add("Japan");
// countries.Add("china");

// countries.Remove("USA");
// countries.RemoveAt(1);
// countries.RemoveAll(e => e.Equals("china"));
// foreach (string country in countries)
// {
//     Console.WriteLine(country);
// }

// Dictionary<int,string> dict= new Dictionary<int,string>();
//             dict.Add(1, "India");
//             dict.Add(2, "USA");
//             dict.Add(3, "Japan");
//             dict.Add(4, "China");

//             foreach(KeyValuePair<int,string> kv in dict)
//             {
//                 Console.WriteLine(kv.Key + " " + kv.Value);
//             }

// string filePath = "example.txt";
//  string additionalContent = "This is new content to append.";

//         try
//         {
//             File.AppendAllText(filePath, additionalContent + Environment.NewLine);
//             Console.WriteLine("Text appended to the file.");
//         }
//         catch (IOException ex)
//         {
//             Console.WriteLine($"Error: {ex.Message}");
//         }

// string dirPath = "newDirectory";
// string filePath = dirPath + @"/example.txt";
//  try
//             {
//                 // Create the directory if it doesn't exist
//                 if (!Directory.Exists(dirPath))
//                 {
//                     Directory.CreateDirectory(dirPath);
//                     Console.WriteLine("Directory created successfully.");
//                 }
//                 else
//                 {
//                     Console.WriteLine("Directory already exists.");
//                 }
//             }
//             catch (IOException ex)
//             {
//                 Console.WriteLine($"Error: {ex.Message}");
//             }
//             string[] lines = {
//             "This is the first line.",
//             "This is the second line.",
//             "This is the third line."
//         };

//             try
//             {
//                 File.AppendAllLines(filePath, lines);
//                 Console.WriteLine("Lines written to file.");
//             }
//             catch (IOException ex)
//             {
//                 Console.WriteLine($"Error: {ex.Message}");
//             }


// string dirPath = "C:\\YourDirectory"; // Specify your directory path

//         try
//         {
//             if (Directory.Exists(dirPath))
//             {
//                 // Get all files
//                 string[] files = Directory.GetFiles(dirPath);
//                 Console.WriteLine("Files:");
//                 foreach (var file in files)
//                 {
//                     Console.WriteLine(file);
//                 }

//                 // Get all subdirectories
//                 string[] subdirs = Directory.GetDirectories(dirPath);
//                 Console.WriteLine("\nSubdirectories:");
//                 foreach (var subdir in subdirs)
//                 {
//                     Console.WriteLine(subdir);
//                 }
//             }
//             else
//             {
//                 Console.WriteLine($"Error: Directory '{dirPath}' does not exist.");
//             }
//         }
//         catch (IOException ex)
//         {
//             Console.WriteLine($"Error: {ex.Message}");
//         }

// Hashtable ht = new Hashtable();
// ht.Add(1, "India");
// ht.Add(2, "Japan");
// foreach (DictionaryEntry de in ht)
// {
//     Console.WriteLine(de.Key + " " + de.Value);
// }
// SortedList<int, string> sl = new SortedList<int, string>();
// sl.Add(1, "Country");
// sl.Add(3, "District");
// sl.Add(2, "State");
// foreach (KeyValuePair<int, string> kvp in sl)
// {
//     Console.WriteLine(kvp.Key + " " + kvp.Value);
// }


// ThreadingExample te = new ThreadingExample();
// Thread t1 = new Thread(te.Even);
// Thread t2 = new Thread(te.Odd);

// t2.Start();
// Thread.Sleep(2000);
// t1.Start();
// public class ThreadingExample
// {
//     public void Even()
//     {
//         for (int i = 0; i <= 100; i += 2)
//         {
//             Console.WriteLine(i);
//         }
//     }
//     public void Odd()
//     {
//         for (int i = 1; i <= 20; i += 2)
//         {
//             Console.WriteLine(i);
//         }
//     }
// }

//DelegateExample de = new DelegateExample();

// List<int> results = new List<int>();
// Func<int, int> f1 = de.Square;
// f1 += de.Double;
// f1(10);
// Console.WriteLine(f1(10));
// var Invoclist = f1.GetInvocationList().Cast<Func<int, int>>();
// foreach (var invlist in Invoclist)
// {
//     int result = invlist.Invoke(10);
//     results.Add(result);
// }
// foreach (int i in results)
// {
//     Console.WriteLine(i);
// }

// Action<int> a1 = de.Cube;
// a1(10);
// a1(20);
// public class DelegateExample
// {
//     public int Double(int x)
//     {
//         return x + x;
//     }
//     public int Square(int x)
//     {
//         return x * x;
//     }
//     public void Cube(int x)
//     {
//         Console.WriteLine(x * x * x);
//     }
//     public void Quad(int x)
//     {
//         Console.WriteLine(x * x * x * x);
//     }
// }

//DATABASE CODE-



// EASY (1–10)

// Find length of a string
// Input: "Hello"
// Output: 5
// Console.Write("Enter a string: ");
// string str = Console.ReadLine();
// int length = str.Length;
// Console.WriteLine("The length the string is : " + length);

// // Convert string to uppercase
// // Input: "welcome"
// // Output: "WELCOME"
// Console.Write("Enter a string: ");
// string str1 = Console.ReadLine();
// Console.WriteLine("The string in upper case is : " + str1.ToUpper());

// // Convert string to lowercase
// // Input: "DOTNET"
// // Output: "dotnet"
// Console.Write("Enter a string: ");
// string str2 = Console.ReadLine();
// Console.WriteLine("The string in all lowers is: " + str2.ToLower());

// // Concatenate two strings
// // Input: "Hello", "C#"
// // Output: "Hello C#"
// Console.Write("Enter String 1: ");
// string str3 = Console.ReadLine();
// Console.Write("Enter String 2: ");
// string str4 = Console.ReadLine();
// string strconcat = string.Concat(str3 + " " + str4);
// Console.WriteLine("The new string is : " + strconcat);

// // Check if a string is empty or null
// // Input: ""
// // Output: True
// Console.Write("Enter a string: ");
// string str5 = Console.ReadLine();
// bool check = str5.IsNullOrEmpty();
// if (check)
// {
//     Console.WriteLine(check);
// }
// else
// {
//     Console.WriteLine(check);
// }

// // Get first character of a string
// // Input: "India"
// // Output: 'I'
// Console.Write("Enter a string: ");
// string str6 = Console.ReadLine();
// char ch = str6[0];
// Console.WriteLine("The first character is : " + ch);

// // Get last character of a string
// // Input: "India"
// // Output: 'a'
// Console.Write("Enter a string: ");
// string str7 = Console.ReadLine();
// int length1 = str.Length - 1;
// char ch1 = str7[length1];
// Console.WriteLine("The first character is : " + ch1);

// // Compare two strings (case-sensitive)
// // Input: "abc", "ABC"
// // Output: False
// Console.Write("Enter String 1: ");
// string str8 = Console.ReadLine();
// Console.Write("Enter String 2: ");
// string str9 = Console.ReadLine();
// Console.WriteLine("Are the two strings same? " + str8.CompareTo(str9));

// // Check if string contains a word
// // Input: "Welcome to C#", c
// // Output: True
// string str10 = "Welcome to C#";
// string str11 = "C#";
// Console.WriteLine("Does the string contain 'c#'? " + str10.Contains(str11));

// // Trim leading and trailing spaces
// // Input: " Hello World "
// // Output: "Hello World"
// string str12 = " Hello World ";
// Console.WriteLine("The trimmed string is: " + str12.Trim());

// // 🟡 MEDIUM (11–20)

// // Reverse a string
// // Input: "CSharp"
// // Output: "prahSC"
// Console.Write("Enter a string: ");
// string strmed1 = Console.ReadLine();
// char[] charr = strmed1.ToCharArray();
// Array.Reverse(charr);
// string newstr = new string(charr);
// Console.WriteLine("The reversed string is: " + newstr);

// // Count number of vowels
// // Input: "Education"
// // Output: 5


// Count number of consonants
// Input: "Hello"
// Output: 3

// Check if a string is a palindrome
// Input: "madam"
// Output: True

// Count words in a sentence
// Input: "I love C Sharp"
// Output: 4

// Replace spaces with underscore
// Input: "Full Stack Developer"
// Output: "Full_Stack_Developer"

// Find index of first occurrence of a character
// Input: "programming", 'g'
// Output: 3

// Remove all white spaces from a string
// Input: "C Sharp Language"
// Output: "CSharpLanguage"

// Check if string starts with a substring
// Input: "www.google.com", "www"
// Output: True

// Check if string ends with a substring
// Input: "file.txt", ".txt"
// Output: True

// 🟠 HARD (21–30)

// Count frequency of each character
// Input: "banana"
// Output: b:1, a:3, n:2
// string s = Console.ReadLine();
// Dictionary<char, int> dict = new Dictionary<char, int>();

// foreach (char c in s)
// {
//     if (dict.ContainsKey(c))
//     {
//         dict[c]++;
//     }
//     else
//     {
//         dict[c] = 1;
//     }
// }

// foreach (var item in dict)
// {
//     Console.WriteLine(item.Key + " : " + item.Value);
// }

// Remove duplicate characters from a string
// Input: "programming"
// Output: "progamin"

// Find first non-repeating character
// Input: "swiss"
// Output: 'w'

// Check if two strings are anagrams
// Input: "listen", "silent"
// Output: True

// Capitalize first letter of each word
// Input: "welcome to c sharp"
// Output: "Welcome To C Sharp"

// Find longest word in a sentence
// Input: "C sharp string manipulation"
// Output: "manipulation"

// Count occurrences of a substring
// Input: "abababab", "ab"
// Output: 4

// Extract domain from an email address
// Input: "user@gmail.com"
// Output: "gmail.com"

// Mask all but last 4 characters of a string
// Input: "1234567890"
// Output: "******7890"

// Validate password strength
// Conditions:

// Minimum 8 characters

// At least 1 uppercase

// At least 1 lowercase

// At least 1 digit

// At least 1 special characte    

// Patient p = new Patient();

// Console.Write("Enter MRN: ");
// p.MRN = int.Parse(Console.ReadLine());

// Console.Write("Enter Patient Name: ");
// p.PatientName = Console.ReadLine();

// Console.Write("Enter Age: ");
// p.Age = int.Parse(Console.ReadLine());

// Console.Write("Enter Body Weight: ");
// p.BodyWeight = float.Parse(Console.ReadLine());

// Console.Write("Enter Height: ");
// p.Height = float.Parse(Console.ReadLine());

// Console.Write("Enter MRN to delete: ");
// int MRN = int.Parse(Console.ReadLine());

// PatientCRUD crud = new PatientCRUD();
// List<Patient> allPatients = crud.GetPatientsList();

// foreach (var a in allPatients)
// {
//     Console.WriteLine($"{a.MRN} | {a.PatientName} | {a.Age} | {a.BodyWeight} | {a.Height}");
// }

// Simple (Warm-up)

// 1.Create an array of 5 integers and print all elements.

// 2.Find the sum of all elements in an array.

// 3.Find the average of elements in an array.

// 4.Find the largest element in an array.

// 5.Find the smallest element in an array.

// 6.Count how many elements are even and odd.

// 7.Search for a given number and print its index (or -1 if not found).

// 8.Copy elements from one array to another.

// 9.Reverse an array and print it.

// 10.Print only the elements at even indexes.

// int[] arr = new int[5];
// for(int i=0;i<arr.Length;i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// foreach (int i in arr)
// {
//     Console.WriteLine(arr[i - 1]);
// }


// int[] arr = new int[5];
// int sum = 0;
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
//     sum += arr[i];
// }
// Console.WriteLine("The sum is: " + sum);


// int[] arr = new int[5];
// int sum = 0;
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
//     sum += arr[i];
// }
// int avg = sum / (arr.Length);
// Console.WriteLine("The avg is: " + avg);

// int[] arr = new int[5];
// int max = 0;
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// max = arr[0];
// for (int i = 1; i < arr.Length; i++)
// {
//     if (arr[i] > max)
//     {
//         max = arr[i];
//     }
    
// }
// Console.WriteLine("The max is: " + max);

// int[] arr = new int[5];
// int min = 0;
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// min = arr[0];
// for (int i = 1; i < arr.Length; i++)
// {
//     if (arr[i] < min)
//     {
//         min = arr[i];
//     }
    
// }
// Console.WriteLine("The min is: " + min);

// int[] arr = new int[5];
// int countodd = 0;
// int counteven = 0;
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// for (int i = 0; i < arr.Length; i++)
// {
//     if (arr[i] % 2 == 0)
//     {
//         counteven++;
//     }
//     else
//     {
//         countodd++;
//     }
    
// }
// Console.WriteLine("The even is: " + counteven);
// Console.WriteLine("The odd is: " + countodd);

// int[] arr = new int[5];

// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }

// Console.Write("Enter the number to search: ");
// int num = int.Parse(Console.ReadLine());
// bool check=false;
// for (int i = 0; i < arr.Length; i++)
// {
//     check = (arr[i] == num);
//     if (check)
//     {
//         Console.WriteLine("The index is: " + i);
//     }

// }
// if (!check)
// {
//     Console.WriteLine("-1");
// }


// int[] arr = new int[5];
// int[] arr1 = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }

// for (int i = 0; i < arr.Length; i++)
// {
//     arr1[i] = arr[i];
// }
// Console.WriteLine("Now printing the second array with copied values:");
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.WriteLine(arr1[i]);
// }

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// Console.WriteLine("The array reversed is: ");
// for (int i = arr.Length-1; i >= 0; i--)
// {
//     Console.WriteLine(arr[i]);
// }

// int[] arr = new int[5];

// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }

// Console.WriteLine("Now printing the second array with copied values:");
// for (int i = 1; i < arr.Length; i=i+2)
// {
//     Console.WriteLine(arr[i]);
// }

//Medium (Logic Building)

// 1.Sort an array in ascending order (without using built-in sort).

// 2.Sort an array in descending order.

// 3.Remove duplicate elements from an array.

// 4.Count how many times a given element occurs.

// 5.Find the second largest element.

// 6.Find the second smallest element.

// 7.Rotate an array to the left by 1 position.

// 8.Rotate an array to the right by 1 position.

// 9.Check if an array is sorted.

// 10.Merge two arrays into a third array.


// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }

// for (int i = 0; i < arr.Length - 1; i++)
// {
//     for (int j = 0; j < arr.Length - 1; j++)
//     {
//         if (arr[j] > arr[j + 1])
//         {
//             int temp = arr[j];
//             arr[j] = arr[j + 1];
//             arr[j + 1] = temp;
//         }
//     }

// }
// Console.WriteLine("Sorted array:");
// for (int i = 0; i < arr.Length;i++)
// {
//     Console.WriteLine(arr[i]);
// }

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }

// for (int i = 0; i < arr.Length - 1; i++)
// {
//     for (int j = 0; j < arr.Length - 1; j++)
//     {
//         if (arr[j] < arr[j + 1])
//         {
//             int temp = arr[j];
//             arr[j] = arr[j + 1];
//             arr[j + 1] = temp;
//         }
//     }

// }
// Console.WriteLine("Sorted array:");
// for (int i = 0; i < arr.Length;i++)
// {
//     Console.WriteLine(arr[i]);
// }

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// Array.Sort(arr);
// Console.WriteLine("the array without duplicate:");
// for (int i = 0; i < arr.Length-1;i++)
// {
//     if (arr[i] != arr[i + 1])
//     {
//         Console.WriteLine(arr[i]);
//     }
// }
// Console.WriteLine(arr[arr.Length - 1]);


// int[] arr = new int[5];

// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// for (int i = 0; i < arr.Length - 1; i++)
// {
//     for (int j = 0; j < arr.Length - 1; j++)
//     {
//         if (arr[j] > arr[j + 1])
//         {
//             int temp = arr[j];
//             arr[j] = arr[j + 1];
//             arr[j + 1] = temp;
//         }
//     }

// }
// Console.Write("Enter the number to search: ");
// int num = int.Parse(Console.ReadLine());
// bool check = false;
// int count = 0;
// for (int i = 0; i < arr.Length; i++)
// {
//     check = (arr[i] == num);
//     if (check)
//     {
//         count++;
//     }
// }
// Console.WriteLine("The number is in the array and is present "+count+" times");
// if (count==0)
// {
//     Console.WriteLine("The number is not in the array");
// }

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// int max = int.MinValue;
// int secondMax = int.MinValue;
// for (int i = 0; i < arr.Length;i++)
// {
//     if (arr[i] > max)
//     {
//         secondMax = max;
//         max = arr[i];
//     }
//     else if (arr[i] > secondMax && arr[i] != max)
//     {
//         secondMax = arr[i];
//     }
// }
// Console.WriteLine("The second max is : " + secondMax);

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// int max = int.MinValue;
// int secondMax = int.MinValue;
// for (int i = 0; i < arr.Length;i++)
// {
//     if (arr[i] > max)
//     {
//         secondMax = max;
//         max = arr[i];
//     }
//     else if (arr[i] > secondMax && arr[i] != max)
//     {
//         secondMax = arr[i];
//     }
// }
// Console.WriteLine("The second max is : " + secondMax);

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// int min = int.MaxValue;
// int secondMin = int.MaxValue;
// for (int i = 0; i < arr.Length;i++)
// {
//     if (arr[i] < min)
//     {
//         secondMin = min;
//         min = arr[i];
//     }
//     else if (arr[i] < secondMin && arr[i] != min)
//     {
//         secondMin = arr[i];
//     }
// }
// Console.WriteLine("The second min is : " + secondMin);

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// int first = arr[0];
// for (int i = 0; i < arr.Length - 1; i++)
// {
//     arr[i] = arr[i + 1];
// }
// arr[(arr.Length) - 1] = first;
// Console.WriteLine("The rotated left: ");
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.WriteLine(arr[i]);
// }

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// int last = arr[arr.Length - 1];
// for (int i = arr.Length-2; i >=0; i--)
// {
//     arr[i+1] = arr[i];
// }
// arr[0] = last;
// Console.WriteLine("The rotated left: ");
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.WriteLine(arr[i]);
// }

// int[] arr = new int[5];
// for (int i = 0; i < arr.Length; i++)
// {
//     Console.Write("Enter value: ");
//     arr[i] = int.Parse(Console.ReadLine());
// }
// bool checkasc = true;
// bool checkdsc = true;
// for (int i = 0; i < arr.Length - 1; i++)
// {
//     if (arr[i] > arr[i + 1])
//     {
//         checkasc = false;
//     }
//     if (arr[i] < arr[i + 1])
//     {
//         checkdsc = false;
//     }
// }
// if (checkasc)
// {
//     Console.WriteLine("Array is ascending");
// }
// else if (checkdsc)
// {
//     Console.WriteLine("Array is descending");
// }
// else
// {
//     Console.WriteLine("Array is not sorted");
// }

