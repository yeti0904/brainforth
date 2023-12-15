\ environment
8192 constant PROGRAM_SIZE
3000 constant MEM_SIZE
1024 constant STACK_SIZE
variable ip
create   program PROGRAM_SIZE allot
variable ptr
create   mem MEM_SIZE cells allot
variable op
create   stack STACK_SIZE cells allot
variable sp

: reset
	program ip !
	mem ptr !
	stack sp !
	mem MEM_SIZE erase
	program PROGRAM_SIZE erase
	stack STACK_SIZE erase
;

: read_char ip @ C@ op ! ;
: read_cell ptr @ @ ;
: write_cell ptr @ ! ;
: inc_ip ip @ 1 + ip ! ;
: push_ip ip @ sp @ ! sp @ 1 cells + sp ! ;
: stack_top sp @ 1 cells - @ ;
: stack_pop sp @ 1 cells - sp ! ;

: b+ read_cell 1 + write_cell ;
: b- read_cell 1 - write_cell ;
: b< ptr @ 1 cells - ptr ! ;
: b> ptr @ 1 cells + ptr ! ;
: b. ptr @ @ emit ;
: b, key ptr @ ! ;
: b[
	read_cell 0= if
		1 \ level
		
		begin
			inc_ip
			read_char
			
			op @ '[' = if
				1 +
			then
			
			op @ ']' = if
				1 -
			then
		dup 0= until
	else
		push_ip
	then
;
: b]
	read_cell 0= if
		stack_pop
	else
		stack_top ip !
	then
;

: run_program
	read_char
	
	begin
		op @ '+' = if b+ then
		op @ '-' = if b- then
		op @ '<' = if b< then
		op @ '>' = if b> then
		op @ '.' = if b. then
		op @ ',' = if b, then
		op @ '[' = if b[ then
		op @ ']' = if b] then
		
		inc_ip
		read_char
	op @ 0= until

	cr
;

: main
	begin
		reset
		." Program: "
		program PROGRAM_SIZE accept drop cr
		run_program
	again
;
main
