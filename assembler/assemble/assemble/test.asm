; Instructions
;
; 0		and 	= AND
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

.data:
    i   DW  0
    one   DW  1
    length  DW  5  
    minus1    DW  -1

.text:
    movxi one       ; initialize for loop incrementer
    swp
label forLoop:
    movxi i         ; load i
    swp             ; check if i == length
    movxi minus1    
    mul
    movxi length
    add             
    jz loopEnd
    movxi i         ; increment i by 1
    swp
    movxi one
    add
    movxo i
    jmp forLoop     ; jump to for loop check :)
label loopEnd:
    ret             ; return