using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Spine.Unity;
using System.Linq;

public class SequenceEpisodeNodeObject : EpisodeNodeObject
{
    [SerializeField] private Transform contentParent_;

    private Dictionary<string, Transform> objects = new Dictionary<string, Transform>();

    private SequenceData sequenceData
    {
        get
        {
            return episodeNode_.ProcessedSequenceData;
        }
    }

    public override void Play()
    {
        base.Play();

        GoTweenFlow flow = new GoTweenFlow();

        float startTime = 0f;
        foreach (SequenceData.SequenceStep step in sequenceData.SequenceSteps)
        {
            AddAccompaniment(startTime, flow, step.Accompaniments);

            if (string.Equals(step.SequenceType, SequenceData.SequenceStep.Type.Wait.ToString()))
            {
                flow.insert(startTime, new GoTween(this.transform, step.Duration, new GoTweenConfig()));
                startTime += step.Duration;
            }
            else if (string.Equals(step.SequenceType, SequenceData.SequenceStep.Type.Voiceover.ToString()))
            {
                float audioLength = SimpleAudioPlayer.AudioLength(step.VoiceoverPath);
                string p = step.VoiceoverPath;
                flow.insert(startTime, new GoTween(this.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                {
                    SimpleAudioPlayer.PlayAudio(p);
                })));
                startTime += audioLength;
            }
        }

        flow.insert(startTime, new GoTween(this.transform, 0.01f, new GoTweenConfig().onComplete(t =>
        {
            startLoopCallback_();
        })));
        flow.play();
    }

    private void AddAccompaniment(float startTime, GoTweenFlow flow, List<SequenceData.Accompaniment> accompaniments)
    {
        foreach(SequenceData.Accompaniment accompaniment in accompaniments)
        {
            Transform character = objects[accompaniment.ObjectName];

            startTime = startTime + accompaniment.RelativeTimeAfter;
            startTime = Mathf.Max(startTime, 0f);

            if (accompaniment.Animations != null && accompaniment.Animations.Count > 0)
            {
                SkeletonGraphic s = character.GetComponent<SkeletonGraphic>();

                List<SequenceData.Animation> toPlay = new List<SequenceData.Animation>(accompaniment.Animations);

                flow.insert(startTime, new GoTween(this.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                {
                    s.AnimationState.ClearTrack(Character.kMainTrack);

                    for (int i = 0; i < toPlay.Count; i++)
                    {
                        for (int ii = 0; ii < toPlay[i].LoopTimes; ii++)
                        {
                            s.AnimationState.AddAnimation(Character.kMainTrack, toPlay[i].AnimationName, false, 0.05f);
                            s.AnimationState.AddEmptyAnimation(Character.kMainTrack, toPlay[i].DelayTime, 0.05f);
                        }
                    }
                    s.AnimationState.AddAnimation(Character.kMainTrack, Character.kIdleAnimation, true, 0.05f);
                })));
            }
            if (accompaniment.SoundPath != null && accompaniment.SoundPath.Length > 0)
            {
                string p = accompaniment.SoundPath;
                flow.insert(startTime, new GoTween(this.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                {
                    SimpleAudioPlayer.PlayAudio(p);
                })));
            }

            if (accompaniment.SortingOrder != null)
            {
                int objectIndex = objects[accompaniment.ObjectName].GetSiblingIndex();
                int newIndex = objectIndex;
                if (accompaniment.SortingOrder.SortAbove != null && accompaniment.SortingOrder.SortAbove.Length > 0)
                {
                    newIndex = objects[accompaniment.SortingOrder.SortAbove].GetSiblingIndex() + 1;
                }
                if (accompaniment.SortingOrder.SortBelow != null && accompaniment.SortingOrder.SortBelow.Length > 0)
                {
                    newIndex = objects[accompaniment.SortingOrder.SortBelow].GetSiblingIndex() - 1;
                }

                flow.insert(startTime, new GoTween(this.transform, 0.01f, new GoTweenConfig().onComplete(t =>
                {
                    character.SetSiblingIndex(newIndex);
                })));
            }

            foreach(SequenceData.Movement m in accompaniment.Movements)
            {
                float movementTime = startTime + m.RelativeTimeAfter;
                if (string.Equals(m.MovementType, SequenceData.Movement.Type.Move.ToString()))
                {
                    flow.insert(movementTime, new GoTween(character, m.Duration, new GoTweenConfig().vector3Prop("localPosition", m.Target)));
                } else if (string.Equals(m.MovementType, SequenceData.Movement.Type.Scale.ToString()))
                {
                    flow.insert(movementTime, new GoTween(character, m.Duration, new GoTweenConfig().vector3Prop("localScale", m.Target)));
                } else
                {
                    Debug.LogError("Unhandled movement type: " + m.MovementType);
                }
            }
        }
    }

    public override void Loop()
    {
        base.Loop();

        foreach(Transform t in objects.Values)
        {
            Character c = t.GetComponent<Character>();
            if (c == null) continue;
            c.StartLooping();
        }
        
        foreach(SequenceData.LoopInstructions loopInstruction in sequenceData.Looping)
        {
            Transform character = objects[loopInstruction.ObjectName];
            if (character == null) continue;

            SkeletonGraphic sg = character.GetComponent<SkeletonGraphic>();
            if (sg == null) continue;

            sg.AnimationState.SetAnimation(Character.kMainTrack, loopInstruction.Animation, true);
        }
    }

    public override void Preload(EpisodeNode node)
    {
        base.Preload(node);

        List<SequenceData.Object> objectsToSpawn = new List<SequenceData.Object>();

        if (sequenceData.TemplateId != null && sequenceData.TemplateId.Length > 0)
        {
            Episode.Templates.Template found = episodeNode_.Episode.ProcessedTemplateData.Data.Find(t =>
            {
                return string.Equals(t.Key, sequenceData.TemplateId);
            });
            if (found != null)
            {
                objectsToSpawn.AddRange(found.Objects);
            } else
            {
                Debug.LogError("Could not find template: " + sequenceData.TemplateId);
            }
        }

        objectsToSpawn.AddRange(sequenceData.Objects);

        foreach(SequenceData.Object obj in objectsToSpawn)
        {
            Transform createdObject = null;
            if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Image.ToString()))
            {
                Sprite s = Resources.Load<Sprite>(obj.ModelPath);
                Image i = Resources.Load<Image>(ShareManager.PREFAB_PATH + "single_image_player");

                Image image = GameObject.Instantiate<Image>(i);
                image.sprite = s;
                image.SetNativeSize();

                createdObject = image.GetComponent<Transform>();
            } else if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Prefab.ToString()))
            {
                Debug.LogWarning("Loading prefab as scene is not yet implemented");
            } else if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Video.ToString()))
            {
                Debug.LogWarning("Loading video as scene is not yet implemented");
            } else if (string.Equals(obj.ObjectType, SequenceData.Object.Type.Spine.ToString()))
            {
                SkeletonGraphic l = Resources.Load<SkeletonGraphic>(obj.ModelPath);

                if (l == null) {
                    Debug.LogError("Could not find SkeletonGraphic with path: " + obj.ModelPath);
                }

                SkeletonGraphic sa = GameObject.Instantiate<SkeletonGraphic>(l);
                sa.Skeleton.ScaleX = Mathf.Abs(sa.Skeleton.ScaleX) * (obj.FlipX ? -1 : 1);
                createdObject = sa.GetComponent<Transform>();
            } else
            {
                Debug.LogError("Unhandled scene type: " + obj.ObjectType);
            }

            if (createdObject != null)
            {
                createdObject.transform.SetParent(contentParent_);
                createdObject.transform.localPosition = obj.StartingPosition;
                createdObject.transform.localScale = obj.StartingScale;

                objects.Add(obj.Name, createdObject);
            }
        }
    }
}