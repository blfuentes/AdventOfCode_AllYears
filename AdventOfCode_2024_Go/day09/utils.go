package day09

func checksum(files *[]int) int64 {
	result := 0

	for idx, value := range *files {
		if value > 0 {
			result += idx * value
		}
	}

	return int64(result)
}
