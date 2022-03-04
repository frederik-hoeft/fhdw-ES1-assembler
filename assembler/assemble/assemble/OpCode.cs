using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES1Assembler;

/*
 * ; 0		and 	= AND
; 1		or		= OR
; 2		not		= NOT
; 3		add		= ADD
; 4		rst		= RESET
; 5		mul		= MUL
; 6		N/A 	= RESERVED
; 7		N/A 	= RESERVED
; 8		movxo	= MOV_REGRAM
; 9		movxi 	= MOV_RAMREG
; A		swp 	= MOV_REGREG
; B		jmp		= Jump
; C		jc		= JumpIfCarry
; D		jz		= JumpIfZero
; E		jo		= JumpIfOverflow
; F		ret 	= END
 */
internal enum OpCode : byte
{
    AND = 0x0,
    OR = 0x1,
    NOT = 0x2,
    ADD = 0x3,
    RST = 0x4,
    MUL = 0x5,
    // RESERVED = 0x6
    // RESERVED = 0x7
    MOVXO = 0x8,
    MOVXI = 0x9,
    SWP = 0xA,
    JMP = 0xB,
    JC = 0xC,
    JZ = 0xD,
    JO = 0xE,
    RET = 0xF
}
