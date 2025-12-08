package utilities

// Set is a generic set data structure
type Set[T comparable] struct {
	items map[T]struct{}
}

// NewSet creates a new empty set
func NewSet[T comparable]() *Set[T] {
	return &Set[T]{
		items: make(map[T]struct{}),
	}
}

// NewSetWithValues creates a new set with initial values
func NewSetWithValues[T comparable](values ...T) *Set[T] {
	s := NewSet[T]()
	for _, v := range values {
		s.Add(v)
	}
	return s
}

// Add adds an element to the set
func (s *Set[T]) Add(item T) {
	s.items[item] = struct{}{}
}

// Remove removes an element from the set
func (s *Set[T]) Remove(item T) {
	delete(s.items, item)
}

// Contains checks if an element exists in the set
func (s *Set[T]) Contains(item T) bool {
	_, exists := s.items[item]
	return exists
}

// Size returns the number of elements in the set
func (s *Set[T]) Size() int {
	return len(s.items)
}

// Clear removes all elements from the set
func (s *Set[T]) Clear() {
	s.items = make(map[T]struct{})
}

// IsEmpty returns true if the set has no elements
func (s *Set[T]) IsEmpty() bool {
	return len(s.items) == 0
}

// Values returns a slice of all elements in the set
func (s *Set[T]) Values() []T {
	values := make([]T, 0, len(s.items))
	for item := range s.items {
		values = append(values, item)
	}
	return values
}

// Union returns a new set with elements from both sets
func (s *Set[T]) Union(other *Set[T]) *Set[T] {
	result := NewSet[T]()
	for item := range s.items {
		result.Add(item)
	}
	for item := range other.items {
		result.Add(item)
	}
	return result
}

// Intersection returns a new set with elements common to both sets
func (s *Set[T]) Intersection(other *Set[T]) *Set[T] {
	result := NewSet[T]()
	for item := range s.items {
		if other.Contains(item) {
			result.Add(item)
		}
	}
	return result
}

// Difference returns a new set with elements in s but not in other
func (s *Set[T]) Difference(other *Set[T]) *Set[T] {
	result := NewSet[T]()
	for item := range s.items {
		if !other.Contains(item) {
			result.Add(item)
		}
	}
	return result
}
