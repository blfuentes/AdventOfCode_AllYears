package utilities

// Queue represents a simple queue
type Queue[T any] struct {
	data []T
}

// Enqueue adds an element to the end of the queue
func (q *Queue[T]) Enqueue(value T) {
	q.data = append(q.data, value)
}

// Dequeue removes and returns the element at the front of the queue
// Returns the zero value of T if the queue is empty
func (q *Queue[T]) Dequeue() (T, bool) {
	if len(q.data) == 0 {
		var zero T
		return zero, false
	}
	element := q.data[0]
	q.data = q.data[1:]
	return element, true
}

// Peek returns the element at the front without removing it
func (q *Queue[T]) Peek() (T, bool) {
	if len(q.data) == 0 {
		var zero T
		return zero, false
	}
	return q.data[0], true
}

// IsEmpty checks if the queue is empty
func (q *Queue[T]) IsEmpty() bool {
	return len(q.data) == 0
}

// Size returns the number of elements in the queue
func (q *Queue[T]) Size() int {
	return len(q.data)
}
