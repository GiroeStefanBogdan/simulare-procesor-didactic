using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Nume_proiect
{
    public class Assembler
    {
        protected String filename;
        protected string outputFileName = "";
        // protected string[] outputFile =new string[1000];
        List<string> outputFile = new List<string>();
        protected int outputFileIndex = 0;
        protected Dictionary<string, string> twoOperandsInstructions = new Dictionary<string, string>() {
            //Instructiuni cu 2 operanzi
            {"MOV",          "0000"},
            {"ADD",          "0001"},
            {"SUB",          "0010"},
            {"CMP",          "0111"},
            {"AND",          "1000"},
            {"OR",           "0101"},
            {"XOR",          "0110"}
        };
        protected Dictionary<string, string> oneOperandInstructions = new Dictionary<string, string>() {
            //instructiuni cu un operand
            {"CALL",         "1000001110"},
            {"RRC",          "1000001010"},
            {"INC",          "1000000010"},
            {"ASL",          "1000000100"},
            {"ROL",          "1000000111"},
            {"PUSH",         "1000001100"},
            {"LSR",          "1000000110"},
            {"NEG",          "1000000001"},
            {"ROR",          "1000001000"},
            {"RLC",          "1000001001"},
            {"DEC",          "1000000011"},
            {"JMP",          "1000001011"},
            {"ASR",          "1000000101"},
            {"POP",          "1000001101"},
            {"CLR",          "1000000000"}

        };
        protected Dictionary<string, string> branchInstructions = new Dictionary<string, string>() {
            //instructiuni de salt
            {"BPL",          "11000011"},
            {"BVC",          "11001000"},
            {"BCC",          "11000110"},
            {"BVS",          "11000111"},
            {"BMI",          "11000100"},
            {"BCS",          "11000101"},
            {"BR",           "11000000"},
            {"BEQ",          "11000010"},
            {"BNE",          "11000001"}
        };
        protected Dictionary<string, string> otherInstructions = new Dictionary<string, string>() {
            //diverse
            {"POP PC",      "1110000000001111" },
            {"CLV",         "1110000000000001" },
            {"CLZ",         "1110000000000010" },
            {"CLS",         "1110000000000011" },
            {"CCC",         "1110000000000100" },
            {"WAIT",        "1110000000001101" },
            {"SEV",         "1110000000000110" },
            {"POP FLAG",    "1110000000010001" },
            {"SES",         "1110000000001000" },
            {"SEC",         "1110000000000101" },
            {"NOP",         "1110000000001010" },
            {"CLC",         "1110000000000000"},
            {"HALT",        "1110000000001100" },
            {"SCC",         "10000000001001"  },
            {"PUSH PC",     "1110000000001110"},
            {"RET",         "1110000000001011" },
            {"RETI",        "1110000000010010" },
        };
        protected Dictionary<string, string> registers = new Dictionary<string, string>() {
            {"R0", "0000" },
            {"R1", "0001" },
            {"R2", "0010" },
            {"R3", "0011" },
            {"R4", "0100" },
            {"R5", "0101" },
            {"R6", "0110"},
            {"R7", "0111" },
            {"R8", "1000" },
            {"R9", "1001" },
            {"R10", "1010" },
            {"R11", "1011"},
            {"R12", "1100"},
            {"R13", "1101" },
            {"R14", "1110"},
            {"R15", "1111" }
        };
        protected Dictionary<string, int> etichete = new Dictionary<string, int>();

        protected AssemblerMainForm amf;

        protected int SS = 0;
        protected int DS = 0;
        protected int CS = 0;

        protected List<string> liniiFisierIntrare = new List<string>();
        protected List<Int32> variabileDB = new List<Int32>();
        protected List<Int32> variabileDW = new List<Int32>();
        protected List<Int32> variabileDD = new List<Int32>();



        protected int noOfVariables = 0;
        protected int nrEtichete = 0;
        protected string instructionCode;
        // protected byte instructionCode;
        protected int indexedCode;
        protected int indexedCode2;
        protected int etJumpValue;
        protected int PC = 0;

        public Assembler(AssemblerMainForm f)
        {
            this.amf = f;
        }
        public Assembler() { }

        public String getFileName(String filter)
        {
            try
            {
                /* Local variable used to store the filename */
                String fileNameWithPath = "";
                /* Instantiate an OpenFileDialog */
                OpenFileDialog of = new OpenFileDialog();
                /* Set the filter */
                of.Filter = filter;
                /* Display the Open File dialog */
                if (of.ShowDialog() == DialogResult.OK)
                {
                    /* Get only the filename with full path */
                    fileNameWithPath = of.FileName;
                    /* Get only the filename without path */
                    filename = of.SafeFileName;
                }
                /* Return the filename with complete path */
                return fileNameWithPath;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public void OpenTestFile(string filename)
        {
            amf.ASMShowTextBox.Text = "";
            outputFileName = filename.Substring(0, filename.Length - 3) + "bin";
            try
            {
                StreamReader stream = new StreamReader(filename);
                string line = null;
                while (!stream.EndOfStream)
                {
                    line = stream.ReadLine().Trim();
                    liniiFisierIntrare.Add(line);
                    amf.ASMShowTextBox.AppendText(line);
                    amf.ASMShowTextBox.AppendText("\r\n");
                }
                stream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void generateBinaryFile()
        {
            foreach (string s in liniiFisierIntrare)
            {
                string[] split;
                int index = s.IndexOf(';'); //comentariu               
                if (index > 0)
                {
                    split = s.Substring(0, index).Split(' ', '\n');
                }
                else if (index == 0)
                {
                    continue;
                }
                else
                {
                    split = s.Split(' ', '\n');
                }

                //directivele .stack, .data, .code
                if (split[0][0] == '.')
                {
                    if (split[0].ToLower().Contains("stack"))
                    {
                        if (split.Length != 1)
                        {
                            if (split[1].Substring(split[1].Length - 1, 1).ToLower() == "h")
                            {
                                //dimensiunea stivei e declarata de utilizator si e in hexa
                                SS = Convert.ToInt32(split[1].Substring(0, split[1].Length - 1));
                                amf.ResultTextBox.AppendText("Dimensiunea Stivei(SS) " + SS.ToString());
                            }
                            else
                            {
                                //dimensiunea stivei e declarata de utilizator, dar nu e in hexa
                                SS = Convert.ToInt32(split[1], 16);
                                amf.ResultTextBox.AppendText("Dimensiunea Stivei(SS) " + SS.ToString());
                            }

                        }
                        else
                        {
                            //dimensiunea stivei standard
                            SS = Convert.ToInt32(1024);
                            amf.ResultTextBox.AppendText("Dimensiunea Stivei(SS) " + SS.ToString());
                        }
                    }
                    else if (split[0].ToLower().Contains("data"))
                    {
                        DS = 0;
                    }
                    else if (split[0].ToLower().Contains("code"))
                    {
                        CS = 0;
                    }
                }

                else if (split[0].ToLower().Contains("end"))
                {
                    break;
                }

                //variabile
                else if (split[0].Contains("#"))
                {
                    switch (split[1].ToUpper())
                    {
                        case "DB":
                            if (split[2].Substring(split[2].Length - 1, 1).ToLower() == "h")
                            {
                                variabileDB.Add(Convert.ToInt32(split[2].Substring(0, split[2].Length - 1)));
                            }
                            else
                                variabileDB.Add(Convert.ToInt32(split[2], 16));
                            noOfVariables++;
                            break;
                        case "DW":
                            if (split[2].Substring(split[2].Length - 1, 1).ToLower() == "h")
                            {
                                variabileDW.Add(Convert.ToInt32(split[2].Substring(0, split[2].Length - 1)));
                            }
                            else
                                variabileDW.Add(Convert.ToInt32(split[2], 16));
                            noOfVariables++;
                            break;
                        case "DD":
                            if (split[2].Substring(split[2].Length - 1, 1).ToLower() == "h")
                            {
                                variabileDD.Add(Convert.ToInt32(split[2].Substring(0, split[2].Length - 1)));
                            }
                            else
                            {
                                variabileDD.Add(Convert.ToInt32(split[2], 16));
                            }
                            noOfVariables++;
                            break;
                        default:
                            break;
                    }
                }

                //eticheta
                else if (split[0].Contains("*"))
                {
                    etichete.Add(split[0].Substring(0, split[0].Length - 1), CS + 1);
                    nrEtichete++;
                }

                //instructiune
                else
                {
                    CS++;
                    PC = PC + 2;
                    instructionCode = "5";
                    indexedCode = 0;
                    indexedCode2 = 0;

                    //verificare daca e instr cu 2 operanzi

                    foreach (KeyValuePair<string, string> kvp in twoOperandsInstructions)
                    {
                        if (split[0].ToUpper().Equals(kvp.Key))
                        {

                            string[] splitParametersByComma = split[1].Split(',', ' ');

                            //pun codul intructiunii in instructionCode
                            //  
                            instructionCode = kvp.Value;   //OPCODE

                            if (splitParametersByComma[1].ToUpper().Substring(0, 1) == "R")
                            {

                                //pun modul de adresare pt primul operand (01)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "01";
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (splitParametersByComma[1].ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode.Substring(0, instructionCode.Length) + k.Value;     //opcode+01+R1
                                        break;
                                    }
                                }
                            }

                            //mod indirect pt primul operand
                            else if (splitParametersByComma[1].Substring(0, 1) == "(" && splitParametersByComma[1].Substring(splitParametersByComma[1].Length - 1, 1) == ")")
                            {
                                //pun modul de adresare pt primul operand (10)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "10";
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (splitParametersByComma[1].Substring(1, splitParametersByComma[1].Length - 2).ToUpper() == k.Key)         //opcode+10+R1
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode.Substring(0, instructionCode.Length) + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la final pt primul operand
                            else if ((splitParametersByComma[1].Substring(0, 1) == "(" && splitParametersByComma[1].Substring(splitParametersByComma[1].Length - 1, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun modul de adresare pt primul operand (11)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "11";
                                indexedCode = Convert.ToInt32(splitParametersByComma[1].Substring(splitParametersByComma[1].IndexOf(')') + 1));  //wtf ???   
                                PC += 2;
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (splitParametersByComma[1].Substring(splitParametersByComma[1].IndexOf('(') + 1, splitParametersByComma[1].IndexOf(')') - 1).ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode.Substring(0, instructionCode.Length) + k.Value;       //opcode+11+R1
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la inceput pt primul operand 
                            else if ((splitParametersByComma[1].Substring(splitParametersByComma[1].Length - 1, 1) == ")" && splitParametersByComma[1].Substring(0, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun modul de adresare pt primul operand (11)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "11";
                                indexedCode = Convert.ToInt32(splitParametersByComma[1].Substring(0, splitParametersByComma[1].IndexOf('(')));              //????
                                PC += 2;
                                string[] getRegister = splitParametersByComma[1].Split('(', ')');
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (getRegister[1].ToUpper() == k.Key)
                                    {                                                                                                   //CE II AICI ?
                                        //pun valoarea registrului
                                        instructionCode = instructionCode.Substring(0, instructionCode.Length) + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod imediat
                            else
                            {
                                //pun modul de adresare pt primul operand (00)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "00";
                                indexedCode = Convert.ToInt32(splitParametersByComma[1]);                           //???
                                PC += 2;
                            }

                            //verificare mod de adresare pt destinatie
                            //mod direct
                            if (splitParametersByComma[0].Substring(0, 1) == "R")
                            {
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "01";
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (splitParametersByComma[0].ToUpper() == k.Key)
                                    {
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indirect
                            else if (splitParametersByComma[0].Substring(0, 1) == "(" && splitParametersByComma[0].Substring(splitParametersByComma[0].Length - 1, 1) == ")")
                            {
                                //pun (10) pt mod de adr
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "10";
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (splitParametersByComma[0].Substring(1, splitParametersByComma[0].Length - 2).ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la final
                            else if ((splitParametersByComma[0].Substring(0, 1) == "(" && splitParametersByComma[0].Substring(splitParametersByComma[0].Length - 1, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun (11)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "11";
                                indexedCode2 = Convert.ToInt32(splitParametersByComma[0].Substring(splitParametersByComma[0].IndexOf(')') + 1));
                                PC += 2;
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (splitParametersByComma[0].Substring(splitParametersByComma[0].IndexOf('(') + 1, splitParametersByComma[0].IndexOf(')') - 1).ToUpper() == k.Key)
                                    {
                                        //pun val registrului
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la inceput
                            else if ((splitParametersByComma[0].Substring(splitParametersByComma[0].Length - 1, 1) == ")" && splitParametersByComma[0].Substring(0, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun(11)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "11";
                                indexedCode2 = Convert.ToInt32(splitParametersByComma[0].Substring(0, splitParametersByComma[0].IndexOf('(')));
                                PC += 2;
                                string[] getRegister = splitParametersByComma[0].Split('(', ')');
                                foreach (KeyValuePair<string, string> k in registers)
                                {
                                    if (getRegister[1].ToUpper() == k.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + k.Value;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Cod Ilegal");
                            }
                        }
                    }


                    //instructiuni cu un operand

                    foreach (KeyValuePair<string, string> k in oneOperandInstructions)
                    {
                        if (split[0].ToUpper().Equals(k.Key) && split[1].ToUpper() != "PC" && split[1].ToUpper() != "FLAG")
                        {
                            instructionCode = instructionCode + k.Value;

                            //verificare mod de adresare pt operand

                            //mod direct
                            if (split[1].ToUpper().Substring(0, 1) == "R")
                            {
                                //pun (01)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "01";
                                foreach (KeyValuePair<string, string> kk in registers)
                                {
                                    if (split[1].ToUpper() == kk.Key)
                                    {
                                        //pun val registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indirect
                            else if (split[1].Substring(0, 1) == "(" && split[1].Substring(split[1].Length - 1, 1) == ")")
                            {
                                //pun (10)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "10";

                                foreach (KeyValuePair<string, string> kk in registers)
                                {
                                    if (split[1].Substring(1, split[1].Length - 2).ToUpper() == kk.Key)
                                    {
                                        //pun val. registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la sfarsit
                            else if (split[1].Substring(0, 1) == "(" && split[1].Substring(split[1].Length - 1, 1).Any(c => char.IsDigit(c)))
                            {
                                //pun (11)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "11";
                                indexedCode = Convert.ToInt32(split[1].Substring(split[1].IndexOf(')') + 1));
                                PC += 2;
                                foreach (KeyValuePair<string, string> kk in registers)
                                {
                                    if (split[1].Substring(split[1].IndexOf('(') + 1, split[1].IndexOf(')') - 1).ToUpper() == kk.Key)
                                    {
                                        //pun val registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod indexat cu indexul la inceput
                            else if ((split[1].Substring(split[1].Length - 1, 1) == ")" && split[1].Substring(0, 1).Any(c => char.IsDigit(c))))
                            {
                                //pun (11)
                                instructionCode = instructionCode.Substring(0, instructionCode.Length) + "11";
                                indexedCode = Convert.ToInt32(split[1].Substring(0, split[1].IndexOf('(')));
                                PC += 2;
                                string[] getRegister = split[1].Split('(', ')');
                                foreach (KeyValuePair<string, string> kk in registers)
                                {
                                    if (getRegister[1].ToUpper() == kk.Key)
                                    {
                                        //pun valoarea registrului
                                        instructionCode = instructionCode + kk.Value;
                                        break;
                                    }
                                }
                            }

                            //mod imediat
                            else
                            {
                                MessageBox.Show("Code ilegal!");
                            }
                        }
                    }

                    //instructiuni de branch


                    foreach (KeyValuePair<string, string> k in branchInstructions)
                    {
                        if (split[0].ToUpper().Equals(k.Key))
                        {
                            instructionCode = instructionCode + k.Value;
                            bool found = false;
                            foreach (KeyValuePair<string, int> et in etichete)
                            {
                                if (split[1].Equals(et.Key.Substring(1, et.Key.Length - 1)))
                                {
                                    etJumpValue = et.Value * 2;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                MessageBox.Show("Eticheta incorecta!");
                            }
                            else
                            {
                                if (CS > etJumpValue)
                                {
                                    amf.ResultTextBox.AppendText("PC " + PC.ToString());
                                    instructionCode = instructionCode + ((~(PC) + 1) & 0x00FF) + etJumpValue;
                                    //complement type 2 AND 0x00FF 
                                    amf.ResultTextBox.AppendText(Environment.NewLine);
                                    amf.ResultTextBox.AppendText("JUMP: " + instructionCode.ToString());
                                }
                                else
                                {
                                    instructionCode = instructionCode + PC;
                                    amf.ResultTextBox.AppendText(Environment.NewLine);
                                    amf.ResultTextBox.AppendText(instructionCode.ToString());
                                }
                            }
                        }
                    }

                    //alte instructiuni
                    if (split.Length.Equals(1))
                    {
                        foreach (KeyValuePair<string, string> k in otherInstructions)
                        {
                            if (split[0].ToUpper().Equals(k.Key))
                            {
                                instructionCode = k.Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> k in otherInstructions)
                        {

                            if ((split[0].ToUpper() + " " + split[1].ToUpper()).Equals(k.Key))
                            {
                                instructionCode = k.Value;
                                break;
                            }
                        }
                    }


                    outputFile.Add(instructionCode);
                    outputFileIndex++;
                    /*          if (indexedCode != 0)
                           {
                                  outputFile[outputFileIndex] = indexedCode;
                                  outputFileIndex++;
                                  if (indexedCode2 != 0)
                                  {
                                      outputFile[outputFileIndex] = indexedCode2;
                                      outputFileIndex++;
                                  }
                              } */
                }

            }

            for (int i = 0; i < outputFileIndex; i++)
            {
                amf.ResultTextBox.AppendText(Environment.NewLine);
                amf.ResultTextBox.AppendText(outputFile[i]);
                //      amf.ResultTextBox.AppendText(outputFile.Count.ToString());
                //      amf.ResultTextBox.AppendText("-"); 
                //      amf.ResultTextBox.AppendText(i.ToString());
                //     amf.ResultTextBox.AppendText(Environment.NewLine);
            }
            //     writeToBinFile();

            amf.ResultTextBox.AppendText(Environment.NewLine);
            foreach (KeyValuePair<string, int> k in etichete)
            {
                amf.ResultTextBox.AppendText(Environment.NewLine);
                amf.ResultTextBox.AppendText(k.Key + " " + k.Value);
            }

            amf.ResultTextBox.AppendText("CS " + CS.ToString());
        }

        //scriere in fisier .bin
        public void writeToBinFile()
        {
            Stream s = new FileStream(outputFileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(s);
            UInt16 ui;

            for (int i = 0; i < outputFileIndex; i++)
            {
                ui = Convert.ToUInt16(outputFile[i]);
                bw.Write(ui);
            }
            bw.Close();
        }


    }
}
