#include <iostream>

using namespace std;

// Factorial
template <unsigned n>
struct Factorial
{
	static const unsigned value = n * Factorial<n - 1>::value;
};

template<>
struct Factorial<1>
{
	static const unsigned value = 1;
};

// Coallatz Sequence
template <unsigned depth, unsigned seed, bool odd>
struct CollatzBase 
{
};

template <unsigned depth, unsigned seed>
struct CollatzBase<depth, seed, true> : CollatzBase<depth + 1, seed * 3 + 1, (seed * 3 + 1) % 2>
{
};

template <unsigned depth, unsigned seed>
struct CollatzBase<depth, seed, false> : CollatzBase<depth + 1, seed/2, (seed/2) % 2>
{
};

template <unsigned depth>
struct CollatzBase<depth, 1, true> 
{
	static const unsigned count = depth;
};

template <unsigned seed>
struct Collatz : CollatzBase<1, seed, seed % 2>
{
};

// Prime numbers
// I didn't show this one in the video, but it's another example

// General form
// Defaults mean we can use this both to start off with just the number, and then use this as a base class
// We could have done this with the Collatz example, but having separate Collatz and CollatzBase classes was clearer
template <unsigned number, unsigned factor = number - 1, bool isNotFactor = number % factor>
struct Number
{
};

// Specialization when isNotFactor is false, therefore not prime
template <unsigned number, unsigned factor>
struct Number <number, factor, false>
{
	const static bool isPrime = false;
};

// Specialization when isNotFactor is true, so try the next possible factor
// We skip any factors which are not themselves prime
template <unsigned number, unsigned factor>
struct Number <number, factor, true> : Number<number,factor - 1, !Number<factor - 1>::isPrime || (number % (factor - 1))>
{
};

// Specialization when we've gone all the way down to 2 with not factors, so must be prime
template <unsigned number>
struct Number <number, 2, true>
{
	const static bool isPrime = true;
};

// Specialization for 2 itself, so is prime
template <>
struct Number <2, 1, false>
{
	const static bool isPrime = true;
};

// Fibonacci numbers
// Another example I didn't show
template<unsigned index>
struct Fibonacci
{
	// Just add the previous two numbers
	const static unsigned value = Fibonacci<index - 1>::value + Fibonacci<index - 2>::value;
};

// Fibonacci sequence begins with 1, 1
template<>
struct Fibonacci<0>
{
	const static unsigned value = 1;
};

template<>
struct Fibonacci<1>
{
	const static unsigned value = 1;
};

// Functions to make display easier
template <unsigned value>
void showFactorial()
{
	cout << value << "! = " << Factorial<value>::value << endl;
}

template <unsigned seed>
void showCollatz()
{
	cout << "Collatz of " << seed << " = " << Collatz<seed>::count << endl;
}

template <unsigned value>
void showPrime()
{
	cout << value << " is" << (Number<value>::isPrime ? " " : " not ") << "prime." << endl;
}

template <unsigned index>
void showFibonacci()
{
	cout << "Fibonacci number " << index << " is " << Fibonacci<index>::value << "." << endl;;
}

int main() 
{
	showFactorial<2>();
	showFactorial<3>();
	showFactorial<4>();
	showFactorial<5>();
	showFactorial<6>();
	showFactorial<7>();
	showFactorial<8>();
	showFactorial<9>();
	showFactorial<10>();
	showFactorial<11>();
	showFactorial<12>();
	showFactorial<13>();
	
	cout << endl;

	showCollatz<2>();
	showCollatz<3>();
	showCollatz<4>();
	showCollatz<5>();
	showCollatz<6>();
	showCollatz<7>();
	showCollatz<8>();
	showCollatz<9>();
	showCollatz<10>();
	showCollatz<11>();
	showCollatz<12>();
	showCollatz<13>();
	showCollatz<14>();
	showCollatz<15>();
	showCollatz<16>();
	showCollatz<17>();
	showCollatz<18>();
	showCollatz<19>();

	cout << endl;

	showPrime<2>();
	showPrime<3>();
	showPrime<4>();
	showPrime<5>();
	showPrime<6>();
	showPrime<7>();
	showPrime<8>();
	showPrime<9>();
	showPrime<10>();
	showPrime<11>();
	showPrime<12>();
	showPrime<13>();
	showPrime<14>();
	showPrime<15>();
	showPrime<16>();
	showPrime<17>();
	showPrime<18>();
	showPrime<19>();

	cout << endl;

	showFibonacci<0>();
	showFibonacci<1>();
	showFibonacci<2>();
	showFibonacci<3>();
	showFibonacci<4>();
	showFibonacci<5>();
	showFibonacci<6>();
	showFibonacci<7>();
	showFibonacci<8>();
	showFibonacci<9>();
	showFibonacci<10>();
	showFibonacci<11>();
	showFibonacci<12>();
	showFibonacci<13>();
	showFibonacci<14>();
	showFibonacci<15>();
	showFibonacci<16>();
	showFibonacci<17>();
	showFibonacci<18>();
	showFibonacci<19>();
}