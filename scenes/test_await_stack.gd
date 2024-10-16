extends Node


var x: float = 0

# Called when the node enters the scene tree for the first time.
func _ready()-> void:
	play_anim()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func play_anim():
	var tween = create_tween()
	tween.tween_property(self, "x", 1.0, 0.01).set_ease(Tween.EASE_IN)
	tween.tween_property(self, "x", 0.0, 0.01).set_ease(Tween.EASE_IN)
	await tween.finished
	play_anim()
