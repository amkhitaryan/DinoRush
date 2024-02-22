extends Camera2D

var zoomSpeed = 10
var zoomMargin = 0.3

var zoomMin = 0.2
var zoomMax = 3

var zoomPos = Vector2()
var zoomFactor = 1.0
var zoomFactor2 = 1.0

# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	
	


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	#zoom.x = lerp(zoom.x, zoom.x * zoomFactor, zoomSpeed * delta)
	zoom.x = lerp(zoom.x, zoomFactor2, zoomSpeed * delta)
	#zoom.y = lerp(zoom.y, zoom.y * zoomFactor, zoomSpeed * delta)
	zoom.y = lerp(zoom.y, zoomFactor2, zoomSpeed * delta)
#
	#zoom.x = clamp(zoom.x, zoomMin, zoomMax)
	#zoom.y = clamp(zoom.y, zoomMin, zoomMax)

	pass
func _input(event):
	pass
	#if abs(zoomPos.x - get_global_mouse_position().x) > zoomMargin:
		#zoomFactor = 1.0
	#if abs(zoomPos.y - get_global_mouse_position().y) > zoomMargin:
		#zoomFactor = 1.0

	if event is InputEventMouseButton:
		if event.is_pressed():
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				#zoom.x += 0.25
				#zoom.y += 0.25
				zoomFactor2 += 0.25
				#zoomFactor -= 0.01
				#zoomPos = get_global_mouse_position()
			if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				zoomFactor2 -= 0.25
				#zoom.x -= 0.25
				#zoom.y -= 0.25
				#zoomFactor += 0.01
				#zoomPos = get_global_mouse_position()
