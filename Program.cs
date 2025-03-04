using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




public class Regel
{
    public static string verwijder(string invoer)
    {
        invoer = invoer.Replace(",", "");
        return invoer.Replace(".", "");
    }


    public List<string> body = new List<string>();
    public string head;
    public Regel(string headd, List<string> bodyy)
    {
        int teller = 0;
            
        while(teller<bodyy.Count())
        {
            bodyy[teller] = verwijder(bodyy[teller]);
            teller++;
        }
        body = bodyy;
        head = verwijder(headd);
    }
}
public static class Program
{

    public static string verwijder(string invoer)
    {
        return invoer.Replace(".", "");
    }

    public static string dinges(bool invoer)
    {
        if(invoer == true)
        {
            return "true.";
        }
        else
        {
            return "false.";
        }
    }


    public static void Main(string[] args)
    {
        long teller = 0;
        string invoer = Console.ReadLine();
        string[] invoerarray = invoer.Split(' ');
        long aantalclauses = long.Parse(invoerarray[1]);
        long aantalgoals = long.Parse(invoerarray[2]);
        string richtingchaining = invoerarray[3];
        List<string> clauses = new List<string>();
        List<string> goals = new List<string>();
        List<Regel> regels = new List<Regel>();
        int stringaantal = 0;

        //terwijl nog niet alle regels gemaakt zijn
        while (teller< aantalclauses)
        {
            int inputteller = 0;
            
            string input = Console.ReadLine();
            string[] inputsplit = input.Split(' ');
            //vraag input

            //voeg input toe in een tijdelijk opslag

            foreach(string s in inputsplit.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                if(s == "%")
                {
                    break;
                }
                else if(s.Contains("\t"))
                {
                    stringaantal++;
                    clauses.Add(s.Replace("\t", ""));
                }
                else
                {
                    stringaantal++;
                    clauses.Add(s);
                }
                
            }
            //ga elke string in array langs om te kijken of er een punt in zit
            while(inputteller < stringaantal)
            {
                //als er een punt in zit maak je er een regel van en maak je de temp weer leeg
                if (clauses[inputteller].Contains("."))
                {
                    Regel regel = new Regel(clauses[0], clauses.Skip(2).ToList());
                    regels.Add(regel);
                    teller++;
                    inputteller++;
                    stringaantal = 0;
                    clauses.Clear();
                }
                //als er geen punt in zit 
                else
                {
                    inputteller++;
                 
                }
            }
        }
        


        //als body leeg is (dus als het een feit is) maak je de body true dus true->X.
        foreach (Regel regel in regels)
        {


            if (regel.body.Count == 0)
            {
                regel.body.Add("true");
            }

                
        }



        // goals nu inlezen
        long goalsteller = 0;
        while (goalsteller < aantalgoals)
        {
            string goalsinput = Console.ReadLine();
            string[] goalssplit = goalsinput.Split(' ');
            goals.Add(verwijder(goalssplit[1]));
            goalsteller++;

        }

        
        int doelenteller = 0;
        while (doelenteller < goals.Count())
        {
            if(richtingchaining == "f")
            {
                Console.WriteLine(goals[doelenteller] + ". " + forwardchainen(regels, goals[doelenteller]));
                
            }
            else
            {
                HashSet<string> geprobeerd = new HashSet<string>(); //misschien uit while loop halen
                Console.WriteLine(goals[doelenteller] + ". " + dinges(backwardchainen(regels, goals[doelenteller], geprobeerd)));
                
            }
                
            doelenteller++;
        }

           







    }
    public static bool backwardchainen(List<Regel> regels, string doel, HashSet<string> geprobeerd)
    {

        
         List<string> feiten = new List<string>();

        //initeer feitenlijst met dingen die je meteen als true weet
        foreach(Regel regel in regels)
        {
            if (regel.body[0] == "true")
            {
                feiten.Add(regel.head);
            }
        }

        if (feiten.Contains(doel))
        {
            return true;
        }
        if(geprobeerd.Contains(doel) == false)
        {
            geprobeerd.Add(doel);

            foreach(Regel regel in regels.Where(r => r.head == doel))
            {
                foreach(string stringetje in regel.body)
                {
                    if(backwardchainen(regels, stringetje, geprobeerd))
                    {
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }



        

       
        

        return false;

        

    }

    public static string forwardchainen(List<Regel> regels, string doel)
    {


        Dictionary<Regel, long> count = new Dictionary<Regel, long>(); //regel
        Dictionary<string, bool> inferredfacts = new Dictionary<string, bool>();

            
        foreach (Regel regel in regels)//1
        {
            count[regel] = regel.body.Count;
            inferredfacts[regel.head] = false;

            foreach (string stringetje in regel.body)//2
            {
                inferredfacts[stringetje] = false;
            }

                
        }


            


        Queue<string> agenda = new Queue<string>();
        agenda.Enqueue("true");

        while (agenda.Count > 0)
        {
            string eerstvolgende = agenda.Dequeue();
            if (eerstvolgende == doel)
            {
                return "true.";
            }

            else if (inferredfacts[eerstvolgende] == false)
            {
                inferredfacts[eerstvolgende] = true;
                foreach (Regel regel in regels)
                {
                    if (regel.body.Contains(eerstvolgende))
                    {
                        count[regel] -= 1; 

                        if (count[regel] == 0) 
                        {
                            agenda.Enqueue(regel.head);
                        }
                    }
                        
                }
            }



            //455 aima


               
        }
        return "false.";

    }

    }
    

















