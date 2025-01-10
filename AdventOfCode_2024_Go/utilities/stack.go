package utilities

// Stack represents a stack with an underlying slice
type Stack[T any] struct {
	data []T
}

// Push adds an element to the stack
func (s *Stack[T]) Push(value T) {
	s.data = append(s.data, value)
}

// Pop removes and returns the top element from the stack
// Returns the zero value of T if the stack is empty
func (s *Stack[T]) Pop() (T, bool) {
	if len(s.data) == 0 {
		var zero T
		return zero, false
	}
	last := s.data[len(s.data)-1]
	s.data = s.data[:len(s.data)-1]
	return last, true
}

// Peek returns the top element without removing it
func (s *Stack[T]) Peek() (T, bool) {
	if len(s.data) == 0 {
		var zero T
		return zero, false
	}
	return s.data[len(s.data)-1], true
}

// IsEmpty checks if the stack is empty
func (s *Stack[T]) IsEmpty() bool {
	return len(s.data) == 0
}

// Size returns the number of elements in the stack
func (s *Stack[T]) Size() int {
	return len(s.data)
}
