using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nume_proiect
{
    public class Assambler
    {
        protected String filename;
        protected string outputFileName = "";
        protected int[] outputFile = new int[1024];
        protected int outputFileIndex = 0;
        protected Dictionary<string, int> twoOperandsInstructions = new Dictionary<string, int>() {
            //Instructiuni cu 2 operanzi
            {"MOV",         0},
            {"ADD",         1 },
            {"SUB",         2 },
            {"CMP",         3 },
            {"AND",         4 },
            {"OR",          5 },
            {"XOR",         6 }
        };
        protected Dictionary<string, int> oneOperandInstructions = new Dictionary<string, int>() {
            //instructiuni cu un operand
            {"CLR",         512},
            {"NEG",         513},
            {"INC",         514},
            {"DEC",         515},
            {"ASL",         516},
            {"ASR",         517},
            {"LSR",         518},
            {"ROL",         519},
            {"ROR",         520},
            {"RLC",         521},
            {"RRC",         522},
            {"JMP",         523 },
            {"PUSH",        524 },
            {"POP",         525 },
            {"CALL",        526 }
        };
        protected Dictionary<string, int> branchInstructions = new Dictionary<string, int>() {
            //instructiuni de salt
            {"BR",          192 },
            {"BNE",         193 },
            {"BEQ",         194 },
            {"BPL",         195 },
            {"BMI",         196 },
            {"BCS",         197 },
            {"BCC",         198 },
            {"BVS",         199 },
            {"BVC",         200 }
        };
        protected Dictionary<string, int> otherInstructions = new Dictionary<string, int>() {
            //diverse
            {"CLC",         57344 },
            {"CLV",         57345 },
            {"CLZ",         57346 },
            {"CLS",         57347 },
            {"CCC",         57348 },
            {"SEC",         57349 },
            {"SEV",         57350 },
            {"SEZ",         57351 },
            {"SES",         57352 },
            {"SCC",         57353 },
            {"NOP",         57354 },
            {"HALT",        57355},
            {"WAIT",        57356 },
            {"PUSH PC",     57357 },
            {"POP PC",      57358 },
            {"PUSH FLAG",   57359 },
            {"POP FLAG",    57360 },
            {"RET",         57361 },
            {"RETI",        57362 }
        };
        protected Dictionary<string, int> registers = new Dictionary<string, int>() {
            {"R0", 0 },
            {"R1", 1 },
            {"R2", 2 },
            {"R3", 3 },
            {"R4", 4 },
            {"R5", 5 },
            {"R6", 6 },
            {"R7", 7 },
            {"R8", 8 },
            {"R9", 9 },
            {"R10", 10 },
            {"R11", 11 },
            {"R12", 12 },
            {"R13", 13 },
            {"R14", 14 },
            {"R15", 15 }

    }
}
