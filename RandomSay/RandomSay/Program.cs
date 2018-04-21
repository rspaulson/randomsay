using System;
using System.Text;
using System.Collections;


// Verbs always fail for some reason when they get added to sentences.

namespace RandomSay
{
    public abstract class Word
    {
        public string word;
        public int rhymIndex;
        override public string ToString()
        {
            return word;
        }
    }
    public class Article : Word { }
    public class Noun : Word { }
    public class Verb : Word { }
    public class Adjective : Word { }

    class Program
    {
        /*
        public struct Article : Word { public string word; public int rhymIndex; }
        public struct Noun { public string word; public int rhymIndex; }
        public struct Verb { public string word; public int rhymIndex; }
        public struct Adjective { public string word; public int rhymIndex; }
        */
        public struct Line
        {
            public ArrayList words;
            public int numWords;
            public Line(int _numWords)
            {
                numWords = _numWords;
                words = new ArrayList();
                for (int i = 0; i < numWords; i++)
                    words.Add(null);

            }
            public void AddWord(Article art)
            {
                words.Add(art);
            }
            public void AddWord(Noun noun)
            {
                words.Add(noun);
            }
            public void AddWord(Verb verb)
            {
                words.Add(verb);
            }
            public void AddWord(Adjective adj)
            {
                words.Add(adj);
            }
            public void removeWord(int index)
            {
                words.Remove(index);
            }
            override public string ToString()
            {
                string s = "";
                for (int i = 0; i < numWords; i++)
                    s += words[i].ToString() + " ";
                return s.ToString();
            }
        }

        public struct Poem
        {
            public ArrayList lines;
            public int numLines;
            public Poem(int _numLines)
            {
                numLines = _numLines;
                lines = new ArrayList();
            }
            public void AddLine(Line line)
            {
                lines.Add(line);
            }
            override public string ToString()
            {
                string s = "";
                for (int i = 0; i < numLines; i++)
                    s += lines[i].ToString() + "\n";
                return s;
            }
        }

        public static Random rnd;

        static void Main(string[] args)
        {
            rnd = new Random();
            for (int i = 0; i < 10; i++)
                Console.WriteLine(crtPoem(4, 4));
        }

        public static string crtPoem(int numWordsPerLine, int numLines)
        {
            Poem p = new Poem(numLines);
            for (int i = 0; i < numLines; i++)
            {
                Line l = new Line(numWordsPerLine);
                p.AddLine(CrtSentence(p, l, i));
            }
            return p.ToString();
        }

        // this is where things get complicated
        // the first two lines can be anything so long as they are the correct length
        // all following lines need to rhyme, and must know about the previous lines
        public static Line CrtSentence(Poem p, Line l, int lineNumber)
        {
            // the last word can be decided right away (either noun or adj)
            if (rnd.Next(2) == 0)
                l.words[l.numWords - 1] = crtWrd_n_1s();
            else
                l.words[l.numWords - 1] = crtWrd_adj_1s();

            while (!checkLine(p, l, lineNumber))
            {
                for (int i = l.numWords - 2; i > -1; i--)
                    switch (rnd.Next(4))
                    {
                        case 0:
                            l.words[i] = crtWrd_adj_1s();
                            break;
                        case 1:
                            l.words[i] = crtWrd_art();
                            break;
                        case 2:
                            l.words[i] = crtWrd_n_1s();
                            break;
                        case 3:
                            l.words[i] = crtWrd_v_1s();
                            break;
                    }
            }
            return l;
        }

        public static bool checkLine(Poem p, Line l, int lineNumber)
        {
            for (int i = 0; i < l.numWords - 1; i++)
                if (l.words[i] == null)
                    return false;

            // check rhyme
            if (lineNumber >= 2)
            {
                //ADD CODE
            }

            // check grammar rules
            int consecutiveAdj = 0;
            for (int i = 0; i < l.numWords - 1; i++)
            {
                // adjective
                // BEFORE: verb, article, adjective
                // AFTER: noun, adjective
                if (l.words[i].GetType() == typeof(Adjective))
                {
                    if (i - 1 >= 0)
                        if ((l.words[i - 1].GetType() != typeof(Verb) &&
                        l.words[i - 1].GetType() != typeof(Article) &&
                        l.words[i - 1].GetType() != typeof(Adjective) &&
                        consecutiveAdj > 1))
                            return false;
                    if (l.words[i + 1].GetType() != typeof(Noun) &&
                         l.words[i + 1].GetType() != typeof(Adjective))
                        return false;
                    if (consecutiveAdj > 1) // beyond 2, this will not work
                        return false;
                    ++consecutiveAdj;
                }
                // noun
                // BEFORE: verb, articel, adjective
                // AFTER: verb
                if (l.words[i].GetType() == typeof(Noun))
                {
                    if (i - 1 >= 0)
                        if (l.words[i - 1].GetType() != typeof(Verb) &&
                            l.words[i - 1].GetType() != typeof(Article) &&
                            l.words[i - 1].GetType() != typeof(Adjective))
                            return false;
                    if (l.words[i + 1].GetType() != typeof(Verb))
                        return false;
                    consecutiveAdj = 0;
                }

                // verb
                // BEFORE: noun, (adjective?)
                // AFTER: adjective, article
                if (l.words[i].GetType() == typeof(Verb))
                {
                    if (i - 1 >= 0)
                        if (l.words[i - 1].GetType() != typeof(Noun))
                            return false;
                    if (l.words[i + 1].GetType() != typeof(Adjective) &&
                            l.words[i + 1].GetType() != typeof(Article))
                        return false;
                    consecutiveAdj = 0;
                }
                // article
                // BEFORE: verb
                // AFTER: noun, adjective
                if (l.words[i].GetType() == typeof(Article))
                {
                    if (i - 1 >= 0)
                        if (l.words[i - 1].GetType() != typeof(Verb))
                            return false;
                    if (l.words[i + 1].GetType() != typeof(Noun) &&
                            l.words[i + 1].GetType() != typeof(Adjective))
                        return false;
                    consecutiveAdj = 0;
                }
            }
            return true;
        }

        public static void reviseLine(Poem p, Line l)
        {
        }

        public static Article crtWrd_art()
        {
            string[] sa = { "the", "a" };
            Article art = new Article();
            art.rhymIndex = rnd.Next(sa.GetLength(0));
            art.word = sa[art.rhymIndex];
            return art;
        }
        public static Noun crtWrd_n_1s()
        {
            string[,] sa = { { "dog", "frog" }, { "cat", "hat" } };
            Noun n = new Noun();
            n.rhymIndex = rnd.Next(sa.GetLength(1));
            n.word = sa[rnd.Next(sa.GetLength(0)), n.rhymIndex];
            return n;
        }
        public static Verb crtWrd_v_1s()
        {
            string[,] sa = { { "is", "isn't" }, { "was", "wasn't" } };
            Verb v = new Verb();
            v.rhymIndex = rnd.Next(sa.GetLength(1));
            v.word = sa[rnd.Next(sa.GetLength(0)), v.rhymIndex];
            return v;
        }
        public static Adjective crtWrd_adj_1s()
        {
            string[,] sa = { { "cool", "cruel" }, { "shiny", "tiny" } };
            Adjective adj = new Adjective();
            adj.rhymIndex = rnd.Next(sa.GetLength(1));
            adj.word = sa[rnd.Next(sa.GetLength(0)), adj.rhymIndex];
            return adj;
        }

    }
}
