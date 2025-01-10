package day05

func containsRule(left, right int, rules [][]int) bool {
	for rIdx := 0; rIdx < len(rules); rIdx++ {
		if left == rules[rIdx][0] && right == rules[rIdx][1] {
			return true
		}
	}
	return false
}

func IsInOrder(input []int, rules [][]int) bool {
	for i := 0; i < len(input)-1; i++ {
		left, right := input[i], input[i+1]
		if !containsRule(left, right, rules) {
			return false
		}
	}
	return true
}
